using BatchAPI.BatchData;
using BatchAPI.Extensions;
using BatchAPI.Model;
using FluentValidation;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using static BatchAPI.Model.Batch;

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
                c.SwaggerDoc(name: "v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Batch API", Version = "v2" });
                //c.EnableAnnotations();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddDbContextPool<BatchContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeConnectionString")));

            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

            services.AddTransient<IValidator<BatchFile>, BatchFileValidator>();

            services.AddLogging(p =>
            {
                p.AddEventLog(); // Log to event viewer
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {

                options.InvalidModelStateResponseFactory = context =>
                {
                    return CustomErrorResponse(context);
                    //    Exceptions exception = new Exceptions();
                    //    exception.Errors = new List<Errors>();

                    //    exception.CorrelationId = Guid.NewGuid();

                    //    foreach (var err in context.ModelState)
                    //    {
                    //        Errors errors = new Errors();
                    //        errors.Source = err.Key;
                    //        errors.Description = err.Value.Errors.First().ErrorMessage;
                    //        exception.Errors.Add(errors);
                    //    }

                    //    var _logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                    //    var ex = new Exception(context.ModelState.Values.First().Errors.First().ErrorMessage);

                    //    _logger.Log(LogLevel.Error, ex, ex.Message);

                    //    return new BadRequestObjectResult(exception); // Contains the errors to be returned to the client.
                };
            });

            //services.AddCors(options => options.AddDefaultPolicy(
            //    builder=>builder.AllowAnyOrigin()));
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

            app.UseSwagger();
            //app.UseSwagger(c=>
            //{
            //    c.SerializeAsV2 = true;
            //});

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v2/swagger.json", name: "Batch API");

                //c.SwaggerEndpoint("v1/swagger.json", "Batch API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMiddleware<ExceptionMiddlewareExtensions>();

            app.UseExceptionMiddlewareExtensions();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
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

            var _logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
            var ex = new Exception(actionContext.ModelState.Values.First().Errors.First().ErrorMessage);

            _logger.Log(LogLevel.Error, ex, ex.Message);

            return new BadRequestObjectResult(exception);
        }
    }
}
