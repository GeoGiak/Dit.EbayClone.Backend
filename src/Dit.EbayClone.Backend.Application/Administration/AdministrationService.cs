using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.Application.Administration;

public class AdministrationService(ApplicationDbContext applicationDbContext): IAdministrationService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    
    public async Task<Result<string>> ApproveUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        if (user == null)
        {
            return Result<string>.Fail("User not found");
        }
        
        user.Status = UserStatus.Approved;
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<string>.Ok();
    }

    public async Task<Result<string>> RejectUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        if (user == null)
        {
            return Result<string>.Fail("User not found");
        }
        
        user.Status = UserStatus.Rejected;
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<string>.Ok();
    }
}