using Application.UseCases.DeleteContact.Interfaces;
using Domain.Repositories.Relational;
using ErrorOr;

namespace Application.UseCases.DeleteContact;

public class DeleteContactProcessingUseCase(IContactRepository contactRepository) : IDeleteContactProcessingUseCase
{
    public async Task<Error?> Execute(string id, CancellationToken cancellationToken = default)
    {
        var contact = await contactRepository.GetByIdAsync(id, cancellationToken);

        if (contact is not null)
        {
            await contactRepository.DeleteAsync(contact, cancellationToken);
            return null;
        }

        return Error.NotFound();
    }
}
