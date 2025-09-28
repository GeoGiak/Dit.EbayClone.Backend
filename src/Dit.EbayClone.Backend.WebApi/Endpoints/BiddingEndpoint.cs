using Dit.EbayClone.Backend.Application.Bidding;
using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.Extensions;
using Dit.EbayClone.Backend.Core.WebModels.Bids;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.WebApi.Endpoints;

public class BiddingEndpoint
{
    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
    {
        var group = endpoints.MapGroup("bids");
        
        group.MapGet("", GetAllBids).RequireAuthorization();
        group.MapGet("{bidId:guid}", GetBid).RequireAuthorization();
        
        group.MapPost("auction/{auctionId}", Bid).RequireAuthorization("Bidder");
        
        group.MapGet("auction/{auctionId}/bids", GetAllBidsFromAuction).RequireAuthorization("Seller");
        
        return group;
    }

    private static async Task<Results<Ok<Bid>, NotFound>> Bid(
        Guid auctionId,
        BidDto dto,
        IBiddingService biddingService,
        CancellationToken cancellationToken)
    {
        var result = await biddingService.BidAsync(auctionId, dto, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok(result.Data) :
            TypedResults.NotFound();
    }

    private static async Task<Ok<PagedResults<Bid>>> GetAllBids(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        var bids = await dbContext.Bids
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .ToPagedListAsync(page, pageSize, cancellationToken);

        return TypedResults.Ok(bids);
    }
    
    private static async Task<Results<Ok<PagedResults<Bid>>, NotFound>> GetAllBidsFromAuction(
        Guid auctionId,
        ApplicationDbContext dbContext, 
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        var bids = await dbContext.Bids
            .Where(a => a.Id == auctionId)
            .OrderBy(b => b.DateOfBid)
            .ToPagedListAsync(page, pageSize, cancellationToken);
        
        return bids is not null ?
            TypedResults.Ok(bids) :
            TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Bid>, NotFound>> GetBid(
        Guid bidId,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var bid = await dbContext.Bids.FirstOrDefaultAsync(b => b.Id == bidId, cancellationToken: cancellationToken);
        
        return bid is not null ?
            TypedResults.Ok(bid) :
            TypedResults.NotFound();
    }
}