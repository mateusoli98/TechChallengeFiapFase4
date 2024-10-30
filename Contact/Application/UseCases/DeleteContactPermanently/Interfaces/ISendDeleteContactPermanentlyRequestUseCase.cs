using ErrorOr;

namespace Application.UseCases.DeleteContactPermanently.Interfaces;

public interface ISendDeleteContactPermanentlyRequestUseCase
{
    Task<Error?> Execute(string id, CancellationToken cancellationToken = default);
}
