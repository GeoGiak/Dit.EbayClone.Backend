namespace Dit.EbayClone.Backend.Core.WebModels.Categories;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}