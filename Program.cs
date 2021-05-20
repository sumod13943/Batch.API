using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Azure.Identity;
using System;

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
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                            config.AddAzureKeyVault(
                            keyVaultEndpoint,
                            new DefaultAzureCredential());
                        })
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        })
                        .ConfigureLogging((context, logging) =>
                        {
                            // clear default logging providers
                            logging.ClearProviders();
                            logging.AddConfiguration(context.Configuration.GetSection("Logging"));

                            logging.AddEventLog();

                            // add built-in providers manually, as needed 
                            //logging.AddEventLog(p => p.SourceName = "Batch API");
                            //logging.AddEventLog(p => p.LogName = "Logger");
                            logging.AddFilter("Microsoft", LogLevel.Information);

                        });
        }
    }
}
