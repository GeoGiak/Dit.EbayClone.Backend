using Dit.EbayClone.Backend.Core.WebModels.Categories;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Core.Extensions.Mappers.Categories;

public static class CategoriesMapper
{
    public static CategoryDto ModelToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
        };
    }
}