using ErrorOr;

namespace Application.UseCases.DeleteContact.Interfaces;

public interface IDeleteContactProcessingUseCase
{
    Task<Error?> Execute(string id, CancellationToken cancellationToken = default);
}
