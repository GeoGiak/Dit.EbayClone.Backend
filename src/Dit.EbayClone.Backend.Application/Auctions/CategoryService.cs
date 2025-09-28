using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.WebModels.Categories;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.Application.Auctions;

public class CategoryService(ApplicationDbContext dbContext): ICategoryService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public async Task<Result<Category>> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId
        };

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Category>.Ok(category);
    }

    public async Task<Result<Category>> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (category == null)
            return Result<Category>.Fail("Category not found");

        category.Name = dto.Name;
        category.ParentCategoryId = dto.ParentCategoryId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Category>.Ok(category);
    }

    public async Task<Result<bool>> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (category == null)
            return Result<bool>.Fail("Category not found");

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Ok(true);
    }
}