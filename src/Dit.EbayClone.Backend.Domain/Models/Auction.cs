namespace Dit.EbayClone.Backend.Domain.Models;

public class Auction
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Currently { get; set; } = string.Empty;
    public double BuyPrice { get; set; }
    public double FirstBidPrice { get; set; }
    public int NumberOfBids { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public ICollection<AuctionImage> Images { get; set; } = new List<AuctionImage>();
}