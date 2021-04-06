using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog;

namespace WebApp.Data
{
    public static class DataExtensions
    {
        public static void PrepareDB(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                    logger.LogDebug(env.EnvironmentName);
                    if (env.IsDevelopment())
                    {
                        logger.LogDebug("inside env is development");
                        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "An error occurred while migrating or seeding the database.");
                }
            }
        }
    }
}
