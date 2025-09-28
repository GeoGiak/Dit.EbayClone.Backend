namespace Dit.EbayClone.Backend.Core.WebModels.Categories;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Guid? ParentCategoryId { get; set; }
    
    // public List<CategoryDto> SubCategories { get; set; } = new();
}