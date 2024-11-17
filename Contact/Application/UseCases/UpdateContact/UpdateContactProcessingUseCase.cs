using Application.UseCases.UpdateContact.Interfaces;
using Domain.Repositories.Relational;
using Domain.Entities;
using CrossCutting.Utils;

namespace Application.UseCases.UpdateContact;

public class UpdateContactProcessingUseCase(IContactRepository repository) : IUpdateContactProcessingUseCase
{
    private readonly IContactRepository _contactRepository = repository;

    public async Task Execute(Contact updatedContact, CancellationToken cancellationToken = default)
    {
        var contact = await _contactRepository.GetByIdAsync(updatedContact.Id!, cancellationToken);

        if (contact is null)
        {
            throw new Exception("Contato não encontrado."); ;
        }

        var alreadyExists = await Validate(contact, updatedContact, cancellationToken);
        if (!alreadyExists)
        {
            contact.Name = updatedContact.Name;
            contact.Email = updatedContact.Email;
            contact.AreaCode = updatedContact.AreaCode;
            contact.Phone = updatedContact.Phone;
            contact.State = AreaCodeDictionary.GetStateByAreaCode(updatedContact.AreaCode);

            await _contactRepository.UpdateAsync(contact, cancellationToken);
            return;
        }

        throw new Exception("DDD + Telefone informado já está cadastrado no sistema.");
    }

    private async Task<bool> Validate(Contact findedContact, Contact updatedContact, CancellationToken cancellationToken)
    {
        if (updatedContact.Phone != findedContact.Phone || updatedContact.AreaCode != findedContact.AreaCode)
        {
            var alreadyExists = await _contactRepository.Exists(updatedContact.AreaCode, updatedContact.Phone, cancellationToken);

            return alreadyExists;
        }

        return true;
    }
}
