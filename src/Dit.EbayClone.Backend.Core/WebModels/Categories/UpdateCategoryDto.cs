namespace Dit.EbayClone.Backend.Core.WebModels.Categories;

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}