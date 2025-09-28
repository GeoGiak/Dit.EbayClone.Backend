namespace Dit.EbayClone.Backend.Domain.Models;

public class Bid
{
    public Guid Id { get; set; }
    public DateTime DateOfBid { get; set; }
    public double Amount { get; set; }

    public bool IsActive { get; set; } = true;
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public Guid AuctionId { get; set; }
    public Auction Auction { get; set; }
}