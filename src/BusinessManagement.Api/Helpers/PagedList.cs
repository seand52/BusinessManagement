using Microsoft.EntityFrameworkCore;

namespace BusinessManagement.Helpers;

public class PagedList<T>
{
    public PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; set; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    public bool HasNextPage => Page * PageSize < TotalCount;

    public bool HasPreviousPage => Page > 1;
    
    public int PageCount => (int)Math.Ceiling(TotalCount / (double)PageSize);
    

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
        int totalCount = await query.CountAsync();
        var items = await query
            .Skip((page- 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new(items, page, pageSize, totalCount);
    }
}