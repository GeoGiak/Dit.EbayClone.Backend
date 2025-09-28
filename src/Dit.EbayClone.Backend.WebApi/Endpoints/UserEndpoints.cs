using Dit.EbayClone.Backend.Application.Administration;
using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.Extensions;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dit.EbayClone.Backend.WebApi.Endpoints;

public class UserEndpoints
{
    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
    {
        var group = endpoints.MapGroup("users");
        
        group.MapGet("pending", GetPendingUsers).RequireAuthorization("Admin");
        group.MapPut("{userId:guid}/approve", ApproveUser).RequireAuthorization("Admin");
        group.MapPut("{userId:guid}/reject", RejectUser).RequireAuthorization("Admin");
        
        group.MapGet("", GetAllUsers).RequireAuthorization("Admin");
        group.MapGet("{userId:guid}", GetUser).RequireAuthorization("Admin");
        
        return group;
    }

    private static async Task<PagedResults<User>> GetPendingUsers(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        return await dbContext.Users
            .Where(u => u.Status == UserStatus.Pending)
            .OrderBy(u => u.Email)
            .ToPagedListAsync(page, pageSize, cancellationToken);
    }

    private static async Task<Results<Ok, BadRequest<string>>> ApproveUser(
        Guid userId,
        IAdministrationService administrationService,
        CancellationToken cancellationToken)
    {
        var result = await administrationService.ApproveUser(userId, cancellationToken);
        
        return result.Success ? 
            TypedResults.Ok() :
            TypedResults.BadRequest(result.Error);
    }
    
    private static async Task<Results<Ok, BadRequest<string>>> RejectUser(
        Guid userId,
        IAdministrationService administrationService,
        CancellationToken cancellationToken)
    {
        var result = await administrationService.RejectUser(userId, cancellationToken);
        
        return result.Success ? 
            TypedResults.Ok() :
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<PagedResults<User>> GetAllUsers(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        return await dbContext.Users
            .OrderBy(u => u.Email)
            .ToPagedListAsync(page, pageSize, cancellationToken);
    }

    private static async Task<Results<Ok<User>, NotFound>> GetUser(
        Guid userId,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync(userId, cancellationToken);
        
        return user is not null ? TypedResults.Ok(user) : TypedResults.NotFound();
    }
}