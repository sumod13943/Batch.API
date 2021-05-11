using BatchAPI.BatchData;
using BatchAPI.Model;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BatchAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IBatchData, SQLBatchData>();
            //services.AddSingleton<IBatchData, MockBatchData>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Batch API", Version = "V1" });
                c.EnableAnnotations();
                c.IncludeXmlComments(@"D:\Sums\Practice\UKHG\Batch.API\BatchAPI.xml");
            });

            services.AddControllers()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddDbContextPool<BatchContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeConnectionString")));

            services.AddLogging(p =>
            {
                p.AddEventLog(); // Log to event viewer
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    Exceptions exception = new Exceptions();
                    exception.Errors = new List<Errors>();

                    exception.CorrelationId = Guid.NewGuid();

                    foreach (var err in actionContext.ModelState)
                    {
                        Errors errors = new Errors();
                        errors.Source = err.Key;
                        errors.Description = err.Value.Errors.First().ErrorMessage;
                        exception.Errors.Add(errors);
                    }

                    return new BadRequestObjectResult(exception);
                };
            });

            // services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = ctx => new ExceptionMiddlewareExtensions();
            //});

            //services.AddTransient<ExceptionMiddlewareExtensions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseExceptionMiddlewareExtensions();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Batch API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseExceptionMiddlewareExtensions();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
