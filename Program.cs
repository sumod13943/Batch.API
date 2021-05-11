using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        })
                        .ConfigureLogging((context, logging) =>
                        {
                            // clear default logging providers
                            logging.ClearProviders();
                            logging.AddConfiguration(context.Configuration.GetSection("Logging"));

                            // add built-in providers manually, as needed 
                            logging.AddEventLog();

                            //Filtering the below category from logging
                            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);
                            logging.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", LogLevel.Information);
                            logging.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", LogLevel.Information);
                            logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Information);
                            logging.AddFilter("Microsoft.EntityFrameworkCore.Model.Validation", LogLevel.Error);

                        });
        }
    }
}
