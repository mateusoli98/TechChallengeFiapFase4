namespace Domain.DomainObjects.Filters;

public class PaginationResult<T>
{
    public long Total { get; set; }
    public int Count { get; set; }
    public IEnumerable<T> Items { get; set; }

    public PaginationResult(long total, IEnumerable<T> items)
    {
        Total = total;
        Items = items;
        Count = items.Count();
    }
}
