using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Application.Administration;

public interface IAdministrationService
{
    Task<Result<string>> ApproveUser(Guid userId, CancellationToken cancellationToken);
    
    Task<Result<string>> RejectUser(Guid userId, CancellationToken cancellationToken);
}