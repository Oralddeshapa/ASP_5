using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaChair.Models;
using Azure.Identity;
using NLog.Web;

namespace TeaChair
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {

                logger.Debug("init main");
                logger.Error("Test error");

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        SeedData.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occurred while seeding the database.");
                    }

                }

                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
            //
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          })
          .ConfigureLogging(logging =>
          {
              logging.ClearProviders();
              logging.AddDebug();
              logging.AddConsole();
              logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
          })
          .UseNLog();
    }
}
