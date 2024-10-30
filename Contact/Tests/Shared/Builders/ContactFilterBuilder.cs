using Domain.DomainObjects.Filters;

namespace Shared.Builders;

public class ContactFilterBuilder
{
    private readonly ContactFilter _contactFilter;

    public ContactFilterBuilder()
    {
        _contactFilter = new ContactFilter()
        {
            PageNumber = 1,
            PageSize = 10
        };
    }

    public ContactFilterBuilder WithAreaCode(short? areaCode)
    {
        _contactFilter.AreaCode = areaCode;
        return this;
    }

    public ContactFilterBuilder WithPageNumber(int pageNumber)
    {
        _contactFilter.PageNumber = pageNumber;
        return this;
    }

    public ContactFilterBuilder WithPageSize(int pageSize)
    {
        _contactFilter.PageSize = pageSize;
        return this;
    }

    public ContactFilter Build() => _contactFilter;
}
