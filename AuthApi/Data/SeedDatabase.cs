using AuthApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PatientApi.Data;

public static class SeedDatabaseExtensions
{
    public static async Task SeedTestUsersAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var testUsers = new[]
        {
            new IdentityUser { UserName = "user1@medilabo.com", Email = "user1@medilabo.com", EmailConfirmed = true },
        };

        foreach (var user in testUsers)
        {
            if (await userManager.FindByEmailAsync(user.Email!) == null)
            {
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
