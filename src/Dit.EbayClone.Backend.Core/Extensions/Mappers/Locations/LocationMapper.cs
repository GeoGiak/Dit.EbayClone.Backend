using Dit.EbayClone.Backend.Core.WebModels.Locations;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Core.Extensions.Mappers.Locations;

public static class LocationMapper
{
    public static Location LocationDtoToLocation(this LocationDto locationDto)
    {
        return new Location
        {
            Country = locationDto.Country,
            City = locationDto.City,
            Street = locationDto.Street,
            Address = locationDto.Address,
            ZipCode = locationDto.ZipCode
        };
    }
}