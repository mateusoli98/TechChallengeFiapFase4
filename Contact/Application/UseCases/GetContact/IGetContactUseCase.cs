using ErrorOr;

namespace Application.UseCases.GetContact;

public interface IGetContactUseCase
{
    Task<ErrorOr<GetContactResponse>> Execute(string id, CancellationToken cancellationToken = default);
}
