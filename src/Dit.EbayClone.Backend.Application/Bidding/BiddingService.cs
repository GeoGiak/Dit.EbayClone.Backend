using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Bids;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.Application.Bidding;

public class BiddingService(ApplicationDbContext dbContext): IBiddingService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public async Task<Result<Bid>> BidAsync(Guid auctionId, BidDto dto, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId, cancellationToken);

        if (user == null)
        {
            return Result<Bid>.Fail("User not found");
        }
        
        var auction = await _dbContext.Auctions
            .Where(a => a.IsActive)
            .FirstOrDefaultAsync(a => a.Id == auctionId, cancellationToken: cancellationToken);

        if (auction == null)
        {
            return Result<Bid>.Fail("No auction found");
        }

        if (auction.FirstBidPrice >= dto.Amount)
        {
            return Result<Bid>.Fail("Bid is too low");
        }

        if (auction.EndDate < DateTime.UtcNow)
        {
            return Result<Bid>.Fail("End date is too low");
        }
        
        // Create Bid
        var bid = new Bid
        {
            DateOfBid = DateTime.UtcNow,
            Amount = dto.Amount,
            UserId = user.Id,
            AuctionId = auction.Id,
            IsActive = true
        };
        
        // Update auction
        auction.FirstBidPrice = bid.Amount;
        auction.NumberOfBids++;
        
        _dbContext.Bids.Add(bid);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Bid>.Ok(bid);
    }
}