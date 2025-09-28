using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Auctions;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Dit.EbayClone.Backend.Application.Auctions;

public interface IAuctionService
{
    Task<Result<Guid>> CreateAuction(CreateAuctionDto auction, CancellationToken cancellationToken);

    Task<Result<Auction>> UpdateAuctionAsync(Guid id, UpdateAuctionDto dto, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAuctionAsync(Guid id, CancellationToken cancellationToken);
    Task CloseExpiredAuctionsAsync(CancellationToken cancellationToken);
    Task<List<AuctionHistory>> GetUninformedClosedAuctions(CancellationToken cancellationToken);
    Task<Result<bool>> AddImagesAsync(Guid auctionId, IFormFileCollection imagesToInsert, CancellationToken cancellationToken);
    Task<Result<bool>> RemoveImagesAsync(Guid imageId, CancellationToken cancellationToken);
}