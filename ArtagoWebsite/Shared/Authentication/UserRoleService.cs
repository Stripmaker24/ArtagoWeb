using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Shared.Authentication
{
    public class UserRoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<SystemUser> userManager;

        public UserRoleService(RoleManager<IdentityRole> roleManager, UserManager<SystemUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
    }
}
