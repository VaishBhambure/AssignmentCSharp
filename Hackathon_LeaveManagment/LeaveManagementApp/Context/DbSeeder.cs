using Microsoft.AspNetCore.Identity;

namespace LeaveManagementApp.Context
{
    public class DbSeeder
    {
        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Seed Admin
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "vaish@gmail.com",
                    Name = "Vaishnavi "
                };

                await userManager.CreateAsync(adminUser, "Vaish@123");
            }

            // Seed Manager
            if (await userManager.FindByEmailAsync("manager@example.com") == null)
            {
                var managerUser = new User
                {
                    UserName = "manager",
                    Email = "Bhambure@gmail.com",
                    Name = "Bhambure"
                };

                await userManager.CreateAsync(managerUser, "Bhambure@123");
            }
        }
    }
}
