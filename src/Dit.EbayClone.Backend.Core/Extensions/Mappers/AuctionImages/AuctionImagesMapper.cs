using Dit.EbayClone.Backend.Core.WebModels.AuctionImages;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Core.Extensions.Mappers.AuctionImages;

public static class AuctionImagesMapper
{
    public static AuctionImagesDto ModelToDto(this AuctionImage auctionImages)
    {
        return new AuctionImagesDto
        {
            Id = auctionImages.Id,
        };
    }
}