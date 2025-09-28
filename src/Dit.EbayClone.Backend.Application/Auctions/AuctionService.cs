using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.Extensions.Mappers.Bids;
using Dit.EbayClone.Backend.Core.WebModels.Auctions;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dit.EbayClone.Backend.Application.Auctions;

public class AuctionService(
    ApplicationDbContext dbContext,
    ILogger<AuctionService> logger): IAuctionService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<AuctionService> _logger = logger;
    
    public async Task<Result<Guid>> CreateAuction(CreateAuctionDto dto, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId, cancellationToken);
        if (user == null)
        {
            return Result<Guid>.Fail("There is no such user");
        }
        
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<Guid>.Fail("There is no such category");
        }
        
        var auction = new Auction
        {
            Name = dto.Name,
            Description = dto.Description,
            Currently = dto.Currently,
            BuyPrice = dto.BuyPrice,
            StartDate = DateTime.UtcNow,
            EndDate = dto.EndDate,
            IsActive = true,
            UserId = dto.UserId,
            CategoryId = dto.CategoryId,
        };
        
        _dbContext.Auctions.Add(auction);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Ok(auction.Id);
    }

    public async Task<Result<Auction>> UpdateAuctionAsync(
        Guid id, 
        UpdateAuctionDto dto,
        CancellationToken cancellationToken)
    {
        var auction = await _dbContext.Auctions
            // .Include(a => a.Images)
            .Where(a => a.IsActive)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (auction == null)
            return Result<Auction>.Fail("Auction not found");

        auction.Name = dto.Name;
        auction.Description = dto.Description;
        auction.Currently = dto.Currently;
        auction.BuyPrice = dto.BuyPrice;
        auction.EndDate = dto.EndDate;
        auction.CategoryId = dto.CategoryId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<Auction>.Ok(auction);
    }
    
    public async Task<Result<bool>> DeleteAuctionAsync(Guid id, CancellationToken cancellationToken)
    {
        var auction = await _dbContext.Auctions
            // .Include(a => a.Images)
            .Include(a => a.Bids)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (auction == null)
            return Result<bool>.Fail("Auction not found");

        // Remove related bids + images first
        _dbContext.Bids.RemoveRange(auction.Bids);
        // _dbContext.AuctionImages.RemoveRange(auction.Images);
        _dbContext.Auctions.Remove(auction);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> AddImagesAsync(Guid auctionId, IFormFileCollection imagesToInsert, CancellationToken cancellationToken)
    {
        var auction = await _dbContext.Auctions
            .FirstOrDefaultAsync(a => a.Id == auctionId, cancellationToken);

        if (auction == null)
        {
            return Result<bool>.Fail("Auction not found");
        }

        foreach (var file in  imagesToInsert)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);

            var image = new AuctionImage
            {
                Data = ms.ToArray(),
                AuctionId = auctionId,
            };
            
            auction.Images.Add(image);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok();
    }

    public async Task<Result<bool>> RemoveImagesAsync(Guid imageId, CancellationToken cancellationToken)
    {
        var image = await _dbContext.AuctionImages.FirstOrDefaultAsync(image => image.AuctionId == imageId, cancellationToken);

        if (image == null)
        {
            return Result<bool>.Fail("Image not found");
        }
        
        _dbContext.AuctionImages.Remove(image);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Ok();
    }

    public async Task CloseExpiredAuctionsAsync(CancellationToken cancellationToken)
    {
        var expiredAuctions = await dbContext.Auctions
            .Where(a => a.IsActive)
            .Include(a => a.Bids)
            .Where(a => a.EndDate <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var expiredAuction in expiredAuctions)
        {
            var winningBid = expiredAuction.Bids
                .OrderByDescending(b => b.Amount)
                .FirstOrDefault();

            var auctionHistory = new AuctionHistory
            {
                AuctionId = expiredAuction.Id,
                WinnerId = winningBid?.UserId,
                WinningBidAmount = winningBid?.Amount,
                ClosedAt = DateTime.UtcNow
            };
            
            _dbContext.AuctionHistory.Add(auctionHistory);
            expiredAuction.IsActive = false;
            
            foreach (var bid in expiredAuction.Bids)
            {
                bid.IsActive = false;
            }
            _logger.LogInformation("Auction: {expiredAuctionId} has been closed", expiredAuction.Id);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AuctionHistory>> GetUninformedClosedAuctions(CancellationToken cancellationToken)
    {
        var inactiveAuctions = await _dbContext.AuctionHistory
            .Where(ah => !ah.IsNotified)
            .ToListAsync(cancellationToken);

        return inactiveAuctions;
    }
}