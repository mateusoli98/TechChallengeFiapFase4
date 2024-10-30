using ErrorOr;

namespace Application.UseCases.DeleteContact.Interfaces;

public interface ISendDeleteContactRequestUseCase
{
    Task<Error?> Execute(string id, CancellationToken cancellationToken = default);
}
