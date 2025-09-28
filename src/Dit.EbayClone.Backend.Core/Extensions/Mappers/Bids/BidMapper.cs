using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Core.Extensions.Mappers.Bids;

public static class BidMapper
{
    public static BidHistory BidToBidHistory(this Bid bid)
    {
        return new BidHistory()
        {
            DateOfBid = bid.DateOfBid,
            Amount = bid.Amount,
            UserId = bid.UserId,
            AuctionId = bid.AuctionId,
        };
    } 
}