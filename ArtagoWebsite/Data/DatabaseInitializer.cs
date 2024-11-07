using Data.Context;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync(DatabaseContext context, UserManager<SystemUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            int users = await context.Users.CountAsync();
            if (users == 0)
            {
                //create dummy admin account
                var result = await userManager.CreateAsync(new SystemUser() { UserName = "admin" }, "SuperSecurePassword1!");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                var user = await userManager.FindByNameAsync("admin");
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                IdentityRole? role = await roleManager.FindByNameAsync(UserRoles.Admin);

                if (user == null || role?.Name == null)
                {
                    throw new NullReferenceException("");
                }
                await userManager.AddToRoleAsync(user, role.Name);
            }
        }
    }
}
