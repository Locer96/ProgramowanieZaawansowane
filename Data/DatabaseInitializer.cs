using InventoryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryApp.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!await dbContext.Database.CanConnectAsync())
                {
                    await dbContext.Database.MigrateAsync();
                }

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string[] roleNames = { "Administrator", "Support", "User" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                await CreateUsers(userManager, GenerateEmails("admin", 3), "Administrator", "Admin@123");
                await CreateUsers(userManager, GenerateEmails("support", 3), "Support", "Support@123");
                await CreateUsers(userManager, GenerateEmails("user", 25), "User", "User@123");

                // Initialize device models if they don't exist
                string[] pcModels = { "Lenovo", "HP", "Acer" };
                string[] displayModels = { "LG", "HP", "Benq", "Sony" };

                foreach (var model in pcModels)
                {
                    if (!await dbContext.DeviceModels.AnyAsync(dm => dm.Name == model && dm.Type == "PC"))
                    {
                        dbContext.DeviceModels.Add(new DeviceModel { Name = model, Type = "PC" });
                    }
                }

                foreach (var model in displayModels)
                {
                    if (!await dbContext.DeviceModels.AnyAsync(dm => dm.Name == model && dm.Type == "Display"))
                    {
                        dbContext.DeviceModels.Add(new DeviceModel { Name = model, Type = "Display" });
                    }
                }

                await dbContext.SaveChangesAsync();

                // Get random models for workstations
                var pcDeviceModels = await dbContext.DeviceModels.Where(dm => dm.Type == "PC").ToListAsync();
                var displayDeviceModels = await dbContext.DeviceModels.Where(dm => dm.Type == "Display").ToListAsync();
                var random = new Random();

                string[] workStationNumbers = { "A01", "A02", "A03", "A04", "A05", "A06", "A07", "A08", "A09", "A10", "B01", "B02", "B03", "B04", "B05", "B06", "B07", "B08", "B09", "B10", "B11", "B12", "B13", "B14", "B15", "C05", "C06", "C07", "C08", "C09", "C10" };
                foreach (var number in workStationNumbers)
                {
                    if (!await dbContext.WorkStations.AnyAsync(ws => ws.WorkStationNumber == number))
                    {
                        dbContext.WorkStations.Add(new WorkStation
                        {
                            WorkStationNumber = number,
                            PCSerialNumber = GenerateRandomSerialNumber(),
                            PC = pcDeviceModels[random.Next(pcDeviceModels.Count)].Name,
                            Display = displayDeviceModels[random.Next(displayDeviceModels.Count)].Name,
                            Keyboard = true,
                            Mouse = true
                        });
                    }
                }

                await dbContext.SaveChangesAsync();

                var users = await userManager.GetUsersInRoleAsync("User");
                var workStations = await dbContext.WorkStations.ToListAsync();
                for (int i = 0; i < users.Count && i < workStations.Count; i++)
                {
                    var user = users[i];
                    var workStation = workStations[i];
                    if (!await dbContext.AspNetUserWorkStations.AnyAsync(uw => uw.UserId == user.Id && uw.WorkStationId == workStation.Id))
                    {
                        var userWorkStation = new AspNetUserWorkStation
                        {
                            UserId = user.Id,
                            WorkStationId = workStation.Id
                        };
                        dbContext.AspNetUserWorkStations.Add(userWorkStation);
                        workStation.UserWorkStation = userWorkStation;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task CreateUsers(UserManager<IdentityUser> userManager, string[] emails, string role, string password)
        {
            foreach (var email in emails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                    var createUserResult = await userManager.CreateAsync(user, password);
                    if (createUserResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }

        private static string[] GenerateEmails(string prefix, int count)
        {
            var emails = new string[count];
            for (int i = 0; i < count; i++)
            {
                emails[i] = $"{prefix}{i + 1}@test.com";
            }
            return emails;
        }

        private static string GenerateRandomSerialNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                if (i > 0)
                {
                    sb.Append("-");
                }
                for (int j = 0; j < 5; j++)
                {
                    sb.Append(chars[random.Next(chars.Length)]);
                }
            }
            return sb.ToString();
        }
    }
}
