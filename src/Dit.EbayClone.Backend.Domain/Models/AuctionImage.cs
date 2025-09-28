namespace Dit.EbayClone.Backend.Domain.Models;

public class AuctionImage
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public Guid AuctionId { get; set; }
    public Auction Auction { get; set; }
}