namespace Dit.EbayClone.Backend.Core;

public class PagedResults<TSource>
{
    public IReadOnlyList<TSource> Items { get; set; } = [];
    
    public int TotalItems { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int TotalPages { get; set; }
}