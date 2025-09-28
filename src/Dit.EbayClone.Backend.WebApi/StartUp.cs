using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.WebApi;

public static class StartUp
{
    public static void StartApplicationWithAdmin(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!dbContext.Users.Any(u => u.Role == UserRoles.Admin))
        {
            var admin = new User
            {
                Username = "Admin",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@system.local",
                MobileNumber = "0000000000",
                IsActive = true,
                Role = UserRoles.Admin,
                Status = UserStatus.Approved
            };

            dbContext.Users.Add(admin);
            dbContext.SaveChanges();
        }
    }
}