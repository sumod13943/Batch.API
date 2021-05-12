using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

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
                            logging.AddEventLog(p => p.SourceName = "Batch API");
                            logging.AddEventLog(p => p.LogName = "Logger");
                            logging.AddFilter("Microsoft", LogLevel.Information);
                            //Filtering the below category from logging
                            //logging.AddFilter("Microsoft", LogLevel.Information);
                            //logging.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", LogLevel.Information);
                            //logging.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", LogLevel.Information);
                            //logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Information);
                            ////logging.AddFilter("Microsoft.EntityFrameworkCore.Model.Validation", LogLevel.Error);
                            //logging.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Information);
                            //logging.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Information);




                        });
        }
    }
}
