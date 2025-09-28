namespace Dit.EbayClone.Backend.Domain.Models;

public class BidHistory
{
    public Guid Id { get; set; }
    public DateTime DateOfBid { get; set; }
    public double Amount { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public Guid AuctionId { get; set; }
    public AuctionHistory Auction { get; set; }
}