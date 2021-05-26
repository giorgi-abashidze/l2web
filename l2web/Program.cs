using l2web.Data;
using l2web.Data.DataModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await InsertUpdateDataIfNotExists(host);

            await host.RunAsync();
        }

        private static async Task InsertUpdateDataIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    if (context.DataUpdates.Count() == 0) {
                        context.DataUpdates.Add(new DataUpdate());
                        await context.SaveChangesAsync();
                    }

                    var RoleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var UserManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var Configuration = services.GetRequiredService<IConfiguration>();


                    string[] roleNames = { "admin", "user" };


                    foreach (var roleName in roleNames)
                    {
                        var roleExist = await RoleManager.RoleExistsAsync(roleName);
                        if (!roleExist)
                        {
                            await RoleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }

                    string email = Configuration.GetSection("web_admin").GetValue<string>("email");

                    //Here you could create a super user who will maintain the web app
                    var poweruser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        Login = "webadmin",
                        EmailConfirmed = true
                        
                    };

                    var _user = await UserManager.FindByEmailAsync(email);

                    if (_user == null)
                    {
                        string userPWD = Configuration.GetSection("web_admin").GetValue<string>("password");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                        if (createPowerUser.Succeeded)
                        {
                            var acc = new Account();
                            acc.userId = poweruser.Id;
                            acc.User = poweruser;
                            context.GameAccount.Add(acc);

                            poweruser.AccountId = acc.Id;

                            await context.SaveChangesAsync();

                            await UserManager.AddToRoleAsync(poweruser, "admin");

                        }
                    }

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
