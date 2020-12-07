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
using Microsoft.AspNetCore.Identity;

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
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var logging = services.GetRequiredService<ILogger<SeedData>>();

                    try
                    {
                        SeedData.Initialize(services, userManager, logging);
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

        public static async Task Ini(UserManager<User> userManager, NLog.Logger logger)
        {
            User admin = new User
            {
                Points = 85350155,
                Email = "Orald" + "@bsuir.by",
                UserName = "Orald"
            };
            IdentityResult result = await userManager.CreateAsync(admin, "123456");
            logger.Debug(result.ToString());
            if (result.Succeeded)
            {
                logger.Debug("Admin created");
                await userManager.AddToRoleAsync(admin, "user");
                await userManager.AddToRoleAsync(admin, "moder");
                await userManager.AddToRoleAsync(admin, "admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.Debug( error.Description);
                }
            }
        }
    }
}
