using Dit.EbayClone.Backend.Application.Auctions;
using Dit.EbayClone.Backend.Core;
using Dit.EbayClone.Backend.Core.Extensions;
using Dit.EbayClone.Backend.Core.Extensions.Mappers.Categories;
using Dit.EbayClone.Backend.Core.WebModels.Categories;
using Dit.EbayClone.Backend.Domain;
using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.WebApi.Endpoints;

public class ItemCategoryEndpoints
{
    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
    {
        var group = endpoints.MapGroup("category");
        
        group.MapGet("", GetAllCategories);
        group.MapGet("{categoryId:guid}", GetCategory);
        group.MapPost("", CreateCategory);
        group.MapPut("{categoryId:guid}", UpdateCategory);
        group.MapDelete("{categoryId:guid}", DeleteCategory);
        
        return group;
    }
    
    private static async Task<PagedResults<CategoryDto>> GetAllCategories(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 25)
    {
        return await dbContext.Categories
            .OrderBy(c => c.Name)
            .ToPagedListAsync(c => c.ModelToDto(), page, pageSize, cancellationToken);
    }

    private static async Task<Results<Ok<CategoryDto>, NotFound>> GetCategory(
        Guid categoryId,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
        
        return category != null ?
            TypedResults.Ok(category!.ModelToDto()) :
            TypedResults.NotFound();
    }

    private static async Task<Results<Created, BadRequest<string>>> CreateCategory(
        CreateCategoryDto dto,
        ICategoryService categoryService,
        CancellationToken cancellationToken)
    {
        var result = await categoryService.CreateCategoryAsync(dto, cancellationToken);
        
        return result.Success ?
            TypedResults.Created() :
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<Results<Ok, BadRequest<string>>> UpdateCategory(
        Guid categoryId,
        UpdateCategoryDto dto,
        ICategoryService categoryService,
        CancellationToken cancellationToken)
    {
        var result = await categoryService.UpdateCategoryAsync(categoryId, dto, cancellationToken);
        
        return result.Success ? 
            TypedResults.Ok() : 
            TypedResults.BadRequest(result.Error);
    }

    private static async Task<Results<Ok, BadRequest<string>>> DeleteCategory(
        Guid categoryId,
        ICategoryService categoryService,
        CancellationToken cancellationToken)
    {
        var result = await categoryService.DeleteCategoryAsync(categoryId, cancellationToken);
        
        return result.Success ?
            TypedResults.Ok() :
            TypedResults.BadRequest(result.Error);
    }
}
