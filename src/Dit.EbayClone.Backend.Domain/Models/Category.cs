namespace Dit.EbayClone.Backend.Domain.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Self-referencing relationship
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    // Auctions in this category
    public ICollection<Auction> Auctions { get; set; } = new List<Auction>();
}