using ErrorOr;

namespace Application.UseCases.DeleteContactPermanently.Interfaces;

public interface IDeleteContactPermanentlyProcessingUseCase
{
    Task Execute(string id, CancellationToken cancellationToken = default);
}
