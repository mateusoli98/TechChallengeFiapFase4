namespace Domain.DomainObjects.Filters;

public class ContactFilter : PaginationParams
{
    public short? AreaCode { get; set; }
    public string? State { get; set; }
}
