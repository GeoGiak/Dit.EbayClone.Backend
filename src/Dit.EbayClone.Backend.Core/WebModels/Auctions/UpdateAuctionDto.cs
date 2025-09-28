namespace Dit.EbayClone.Backend.Core.WebModels.Auctions;

public class UpdateAuctionDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Currently { get; set; }
    public double BuyPrice { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
}