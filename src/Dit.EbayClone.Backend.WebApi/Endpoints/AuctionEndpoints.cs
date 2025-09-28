using System.Net;
using Dit.EbayClone.Backend.Application.Auctions;
using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.Extensions;
using Dit.EbayClone.Backend.Core.Extensions.Mappers.AuctionImages;
using Dit.EbayClone.Backend.Core.WebModels.AuctionImages;
using Dit.EbayClone.Backend.Core.WebModels.Auctions;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.WebApi.Endpoints;

public class AuctionEndpoints
{
    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
    {
        var group = endpoints.MapGroup("auctions");
        
        group.MapPost("", CreateAuction).RequireAuthorization("Seller");
        group.MapGet("", SearchAuctions);
        group.MapGet("{auctionId:guid}", GetAuction);
        group.MapPut("{auctionId:guid}", UpdateAuction).RequireAuthorization("Seller");
        group.MapDelete("{auctionId:guid}", DeleteAuction).RequireAuthorization("Seller");
        
        group.MapGet("/{auctionId:guid}/image", GetAllAuctionImages);
        group.MapPost("{auctionId:guid}/image", AddAuctionImages).RequireAuthorization("Seller");
        group.MapGet("{auctionId:guid}/image/{imageId:guid}", GetAuctionImage);
        group.MapDelete("{auctionId:guid}/image/{imageId:guid}", RemoveAuctionImage).RequireAuthorization("Seller");
        
        return group;
    }

    private static async Task<Results<Created, BadRequest<string>>> CreateAuction(
        CreateAuctionDto dto,
        IAuctionService auctionService,
        CancellationToken cancellationToken)
    {
        var result = await auctionService.CreateAuction(dto, cancellationToken);
        
        return result.Success ?
            TypedResults.Created() :
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<PagedResults<Auction>> SearchAuctions(
        [AsParameters] SearchAuctionDto dto,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        return await dbContext.Auctions
            .Where(a => a.IsActive)
            .WhereIf(!string.IsNullOrEmpty(dto.AuctionName), x => x.Name.Contains(dto.AuctionName!))
            .WhereIf(dto.MinPrice != null, x => x.FirstBidPrice >= dto.MinPrice)
            .WhereIf(dto.MaxPrice != null, x => x.FirstBidPrice <= dto.MaxPrice)
            .WhereIf(!string.IsNullOrEmpty(dto.CategoryName), x => x.Category.Name.Contains(dto.CategoryName!))
            .WhereIf(dto.StartDate != null, x => x.StartDate >= dto.StartDate)
            .WhereIf(dto.EndDate != null, x => x.EndDate <= dto.EndDate)
            .WhereIf(!string.IsNullOrEmpty(dto.SellerName), x => x.User.Username.Contains(dto.SellerName!))
            .OrderBy(a => a.Id)
            .ToPagedListAsync(page, pageSize, cancellationToken);
    }
    
    private static async Task<Results<Ok<Auction>, NotFound>> GetAuction(
        Guid auctionId,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var auction = await dbContext.Auctions
            .FirstOrDefaultAsync(a => a.Id == auctionId, cancellationToken);
        
        return auction is not null ?
            TypedResults.Ok(auction) :
            TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Auction>, NotFound>> UpdateAuction(
        Guid auctionId,
        UpdateAuctionDto dto,
        IAuctionService auctionService,
        CancellationToken cancellationToken)
    {
        var result = await auctionService.UpdateAuctionAsync(auctionId, dto, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok(result.Data) :
            TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> DeleteAuction(
        Guid auctionId,
        IAuctionService auctionService,
        CancellationToken cancellationToken)
    {
        var result = await auctionService.DeleteAuctionAsync(auctionId, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok() :
            TypedResults.NotFound();
    }

    private static async Task<Results<Ok<PagedResults<AuctionImagesDto>>, NotFound>> GetAllAuctionImages(
        Guid auctionId,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        var images = await dbContext.Auctions
            .Where(a => a.Id == auctionId)
            .Include(a => a.Images)
            .SelectMany(a => a.Images)
            .OrderBy(a => a.Id)
            .ToPagedListAsync(a => a.ModelToDto(), page, pageSize, cancellationToken);

        return images is not null ?
            TypedResults.Ok(images) :
            TypedResults.NotFound();
    }
    
    private static async Task<Results<Ok, NotFound>> AddAuctionImages(
        Guid auctionId,
        HttpRequest request,
        IAuctionService auctionService,
        CancellationToken cancellationToken)
    {
        var form = await request.ReadFormAsync(cancellationToken);
        var files = form.Files;
        
        var result = await auctionService.AddImagesAsync(auctionId, files, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok() :
            TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> RemoveAuctionImage(
        Guid auctionId,
        Guid imageId,
        IAuctionService auctionService,
        CancellationToken cancellationToken)
    {
        var result = await auctionService.DeleteAuctionAsync(imageId, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok() :
            TypedResults.NotFound();
    }

    private static async Task<Results<FileContentHttpResult, NotFound>> GetAuctionImage(
        Guid auctionId,
        Guid imageId,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var image = await dbContext.AuctionImages
            .Where(a => a.AuctionId == auctionId)
            .FirstOrDefaultAsync(i => i.Id == imageId, cancellationToken);

        return image is not null ? 
            TypedResults.File(image.Data) :
            TypedResults.NotFound();
    }
}