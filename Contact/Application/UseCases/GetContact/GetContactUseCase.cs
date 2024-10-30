using Domain.Repositories.Relational;
using ErrorOr;

namespace Application.UseCases.GetContact;

public class GetContactUseCase(IContactRepository contactRepository) : IGetContactUseCase
{
    private readonly IContactRepository _contactRepository = contactRepository;

    public async Task<ErrorOr<GetContactResponse>> Execute(string id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

        if (contact is not null)
        {
            return GetContactResponse.Create(contact);
        }

        return Error.NotFound(description: $"Contato com id: {id} não encontrado. Revise o Id informado ou tente novamente mais tarde");
    }
}
