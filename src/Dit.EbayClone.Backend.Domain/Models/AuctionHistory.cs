namespace Dit.EbayClone.Backend.Domain.Models;

public class AuctionHistory
{
    public Guid Id { get; set; }
    public Guid AuctionId { get; set; }
    public Guid? WinnerId { get; set; }        // Null if no bids
    public double? WinningBidAmount { get; set; }
    public DateTime ClosedAt { get; set; }
    public bool IsNotified { get; set; } = false;

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
}