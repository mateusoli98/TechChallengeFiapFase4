using Application.UseCases.CreateContact.Common;
using ErrorOr;

namespace Application.UseCases.CreateContact.Interfaces;

public interface ISendCreateContactRequestUseCase
{
    Task<ErrorOr<CreateContactResponse>> Execute(CreateContactRequest request, CancellationToken cancellationToken = default);
}
