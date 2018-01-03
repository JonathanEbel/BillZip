using BillZip.StartupHelpers;
using Identity.Infrastructure;
using Identity.Infrastructure.Repos;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BillZip
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            //I'm not a fan of service locator pattern is but it will work for now in this situation....
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<IdentityContext>();
                    var userRepo = services.GetRequiredService<IApplicationUserRepository>();

                    DBSeed.Initialize(context, userRepo);
                }
                catch (Exception ex)
                {
                    //TODO: build some logging....
                    throw new Exception("Failure on bootstrapping.  " + ex.Message);
                }
            }

            host.Run();
            
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
