﻿using Application.UseCases.DeleteContactPermanently.Interfaces;
using Domain.Repositories.Relational;

namespace Application.UseCases.DeleteContactPermanently;

public class DeleteContactPermanentlyProcessingUseCase(IContactRepository contactRepository) : IDeleteContactPermanentlyProcessingUseCase
{
    public async Task Execute(string id, CancellationToken cancellationToken)
    {
        var contact = await contactRepository.GetByIdAsync(id, cancellationToken);

        if (contact is not null)
        {
            await contactRepository.PermanentDelete(contact, cancellationToken);
            return;
        }

        throw new Exception("Contato não encontrado.");
    }
}
