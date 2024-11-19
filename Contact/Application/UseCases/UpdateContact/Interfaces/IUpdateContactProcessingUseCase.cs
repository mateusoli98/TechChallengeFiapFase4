using Domain.Entities;

namespace Application.UseCases.UpdateContact.Interfaces;

public interface IUpdateContactProcessingUseCase
{
    Task Execute(Contact contact, CancellationToken cancellationToken = default);
}
