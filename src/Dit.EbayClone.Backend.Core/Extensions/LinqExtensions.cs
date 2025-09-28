using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.Core.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source, 
        bool condition, 
        Expression<Func<T, bool>> predicate)
        => condition
            ? source.Where(predicate)
            : source;
    public static async Task<PagedResults<TDestination>> ToPagedListAsync<TSource, TDestination>(
        this IOrderedQueryable<TSource> source,
        Expression<Func<TSource, TDestination>> mapper,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (!source.TryGetNonEnumeratedCount(out var count))
        {
            count = await source.CountAsync(cancellationToken);
        }

        var result = new PagedResults<TDestination>
        {
            Items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(mapper)
                .ToListAsync(cancellationToken),
            TotalItems = count,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
        };

        return result;
    }
    
    public static async Task<PagedResults<TSource>> ToPagedListAsync<TSource>(
        this IOrderedQueryable<TSource> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (!source.TryGetNonEnumeratedCount(out var count))
        {
            count = await source.CountAsync(cancellationToken);
        }

        var result = new PagedResults<TSource>
        {
            Items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken),
            TotalItems = count,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
        };

        return result;
    }
}