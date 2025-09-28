namespace Dit.EbayClone.Backend.Domain.Models;

public class Location
{
    public Guid Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}