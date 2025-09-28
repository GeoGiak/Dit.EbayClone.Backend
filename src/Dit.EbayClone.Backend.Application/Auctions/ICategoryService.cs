using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Categories;
using Dit.EbayClone.Backend.Domain.Models;

namespace Dit.EbayClone.Backend.Application.Auctions;

public interface ICategoryService
{
    Task<Result<Category>> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken);
    Task<Result<Category>> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
}