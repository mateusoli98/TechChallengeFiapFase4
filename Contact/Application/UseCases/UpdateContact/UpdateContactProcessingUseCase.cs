using Application.UseCases.UpdateContact.Interfaces;
using CrossCutting.Utils;
using Domain.Entities;
using Domain.Repositories.Relational;

namespace Application.UseCases.UpdateContact;

public class UpdateContactProcessingUseCase(IContactRepository repository) : IUpdateContactProcessingUseCase
{
    private readonly IContactRepository _contactRepository = repository;

    public async Task Execute(Contact updatedContact, CancellationToken cancellationToken = default)
    {
        var contact = await _contactRepository.GetByIdAsync(updatedContact.Id!, cancellationToken);

        if (contact is null)
        {
            return;
        }

        contact.Name = updatedContact.Name;
        contact.Email = updatedContact.Email;
        contact.AreaCode = updatedContact.AreaCode;
        contact.Phone = updatedContact.Phone;
        contact.State = AreaCodeDictionary.GetStateByAreaCode(updatedContact.AreaCode);

        await _contactRepository.UpdateAsync(contact, cancellationToken);
    }
}
