using Application.UseCases.UpdateContact.Common;
using ErrorOr;

namespace Application.UseCases.UpdateContact.Interfaces;

public interface ISendUpdateContactRequestUseCase
{
    Task<ErrorOr<UpdateContactResponse>> Execute(string contactId, UpdateContactRequest request, CancellationToken cancellationToken = default);
}
