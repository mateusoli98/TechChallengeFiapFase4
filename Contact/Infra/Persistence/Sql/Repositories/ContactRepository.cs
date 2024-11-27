using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infra.Persistence.Sql.Context;
using Infra.Extensions;
using Domain.Repositories.Relational;
using Domain.DomainObjects.Filters;
using Domain.Entities;

namespace Infra.Persistence.Sql.Repositories;

public class ContactRepository : IContactRepository
{    
    private DataContext _dataContext;

    public ContactRepository(IServiceScopeFactory scopeFactory)
    {
        _dataContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    }

    public async Task DeleteAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        contact.IsEnabled = false;         
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task PermanentDelete(Contact contact, CancellationToken cancellationToken = default)
    {
        _dataContext.Remove(contact);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<Contact?> GetByIdAsync(string id, CancellationToken cancellationToken = default, bool isEnabled = true)
    {
        var query = _dataContext.Contacts.AsQueryable()
            .Where(q => q.Id == id && q.IsEnabled == isEnabled)
            .FirstOrDefaultAsync(cancellationToken);

        return await query;
    }

    public async Task<string> SaveAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dataContext.Contacts.AddAsync(contact, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            //Logar erro
        }

        return contact.Id;
    }

    public async Task<PaginationResult<Contact>> SearchAsync(ContactFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dataContext.Contacts
            .AsQueryable();

        if (filter.AreaCode > 0)
        {
            query = query.Where(q => q.AreaCode == filter.AreaCode);
        }

        if (!string.IsNullOrEmpty(filter.State))
        {
            query = query.Where(q => q.State == filter.State);
        }

        var result = await query
            .Where(q => q.IsEnabled)
            .OrderBy(q => q.AreaCode)
            .AsNoTracking()
            .ToPaginatedAsync(filter);

        return result;
    }

    public async Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            contact.UpdatedAt = DateTime.UtcNow;
            _dataContext.Contacts.Update(contact);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) 
        {
            //Logar erro
        }
    }

    public async Task<bool> Exists(short areaCode, long phoneNumber, CancellationToken cancellationToken)
    {
        try
        {
            var query = _dataContext.Contacts
                .AsQueryable()
                .Where(q => q.AreaCode == areaCode)
                .Where(q => q.Phone == phoneNumber)
                .Where(q => q.IsEnabled)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return await query is not null;
        }
        catch (Exception ex)
        {
            //Logar erro
            return false;
        }
    }
}
