using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace API.Data
{
    public class UserSeed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole { Name = "CreateRequests" },
                new AppRole { Name = "CreateOutboundOrders" },
                new AppRole { Name = "ModifyParts" },
            };

            foreach (var role in roles)
                await roleManager.CreateAsync(role);

            foreach (var user in users)
            {
                user.UserName = user.Initials;
                await userManager.CreateAsync(user, "0314");
                await userManager.AddToRoleAsync(user, "CreateRequests");
                await userManager.AddToRoleAsync(user, "ModifyParts");
                if (user.FirstName == "Morgan") await userManager.AddToRoleAsync(user, "CreateOutboundOrders");
            }
        }
    }
}