using Domain.DomainObjects.Filters;
using Microsoft.EntityFrameworkCore;

namespace Infra.Extensions;

public static class EntityFrameworkExtensions
{
    public static async Task<PaginationResult<TData>> ToPaginatedAsync<TData>(this IQueryable<TData> queryableResult, PaginationParams paginationParams)
    {
        var itemsCount = queryableResult.Count();
        var skip = (paginationParams.PageNumber - 1) * paginationParams.PageSize;

        var items = await queryableResult.Skip(skip).Take(paginationParams.PageSize).ToListAsync();

        return new PaginationResult<TData>(itemsCount, items);
    }
}
