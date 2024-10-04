using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Admin"
                };

                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.RoleExistsAsync("Member").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Member"
                };

                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("deepakmoningi@gmail.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "Deepak",
                    LastName = "Moningi",
                    Email = "deepak@gmail.com",
                    UserName = "Deepak21",
                    PhoneNumber = "123456789",
                };

                var result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
                }
            }
        }
    }
}
