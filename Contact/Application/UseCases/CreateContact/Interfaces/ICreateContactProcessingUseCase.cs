using Domain.Entities;

namespace Application.UseCases.CreateContact.Interfaces;

public interface ICreateContactProcessingUseCase
{
    Task Execute(Contact contact, CancellationToken cancellationToken = default);
}
