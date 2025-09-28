using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Bids;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Application.Bidding;

public interface IBiddingService
{
    Task<Result<Bid>> BidAsync(Guid auctionId, BidDto dto, CancellationToken cancellationToken);
}