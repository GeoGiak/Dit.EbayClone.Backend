using System.Security.Claims;
using System.Text;
using Dit.EbayClone.Backend.Application.Administration;
using Dit.EbayClone.Backend.Application.Auctions;
using Dit.EbayClone.Backend.Application.Authentication;
using Dit.EbayClone.Backend.Application.BackgroundWorkers;
using Dit.EbayClone.Backend.Application.Bidding;
using Dit.EbayClone.Backend.Application.Notifications;
using Dit.EbayClone.Backend.Core.Options;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dit.EbayClone.Backend.WebApi;

public static class ConfigureServices
{
    public static void AddApplicationDependencies(this WebApplicationBuilder builder)
    {
        builder.AddServices();
        builder.AddBackgroundServices();
        builder.AddOptions();
        builder.AddAuthentication();
        builder.AddAuthorization();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));
    }

    private static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        builder.Services.AddScoped<IAdministrationService, AdministrationService>();
        builder.Services.AddScoped<IAuctionService, AuctionService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IBiddingService, BiddingService>();
        builder.Services.AddScoped<INotificationService, EmailService>();
    }

    private static void AddBackgroundServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<AuctionClosingService>();
        builder.Services.AddHostedService<NotificationSenderService>();
    }

    private static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptionsWithValidateOnStart<JwtOptions>()
            .BindConfiguration(JwtOptions.Section)
            .ValidateDataAnnotations();
        
        builder.Services.AddOptionsWithValidateOnStart<EmailOptions>()
            .BindConfiguration(EmailOptions.Section)
            .ValidateDataAnnotations();
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));
    }

    private static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SigningKey!)
                    )
                };
            });
    }

    private static void AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole(UserRoles.Admin.ToString()));
            options.AddPolicy("Seller", policy => policy.RequireRole(UserRoles.Admin.ToString(), UserRoles.Seller.ToString()));
            options.AddPolicy("Bidder", policy => policy.RequireRole(UserRoles.Admin.ToString(), UserRoles.Bidder.ToString()));
        });
    }
    
}