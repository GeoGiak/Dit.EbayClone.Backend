namespace Dit.EbayClone.Backend.Core.WebModels.Auctions;

public class SearchAuctionDto
{
    public string? AuctionName { get; set; } = string.Empty;
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public string? CategoryName { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SellerName { get; set; } = string.Empty;
}