using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using Cadastro.Domain.Entities;
using Cadastro.Enumerations;
using System.Linq;

namespace Ses.DataSus.Integration.WebSite.App_Start.Database
{
    public static class IdentityDataInitializer
    {
        public static void UseIdentityDataInitializer(this IServiceProvider app)
        {
            using (var scope = app.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                SeedAdministratorUser(userManager, roleManager);
            }
        }

        private static void SeedAdministratorUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            using (var manager = userManager)
            {
                var adminUsername = "admin";
                var user = userManager.FindByNameAsync(adminUsername).Result;
                if (user?.UserName == null)
                {
                    user = new User();
                    user.Name = adminUsername;
                    user.UserName = adminUsername;
                    user.Email = "nat@nat.com";
                    user.EmailConfirmed = true;
                    user.PhoneNumber = "21000000000";
                    user.AccountType = AccountType.Admin;
                    IdentityResult result = userManager.CreateAsync(user, "Natalia@1").Result;
                }

                if (user != null)
                {
                    var adminRoleAsString = AccountType.Admin.ToString();
                    var roles = userManager.GetRolesAsync(user).Result;
                    var admRole = roles?.FirstOrDefault(x => x.Equals(adminRoleAsString, System.StringComparison.InvariantCultureIgnoreCase));
                    if (admRole == null)
                    {
                        var existingRole = roleManager.FindByNameAsync(adminRoleAsString).Result;
                        if (existingRole == null)
                        {
                            var y = roleManager.CreateAsync(new IdentityRole(adminRoleAsString)).Result;
                        }
                        var x = userManager.AddToRoleAsync(user, adminRoleAsString).Result;
                    }
                }
            }
        }
    }
}
