using Application.UseCases.CreateContact.Interfaces;
using Domain.Entities;
using Domain.Repositories.Relational;

namespace Application.UseCases.CreateContact
{
    public class CreateContactProcessingUseCase(IContactRepository repository) : ICreateContactProcessingUseCase
    {
        private readonly IContactRepository _contactRepository = repository;

        public async Task Execute(Contact contact, CancellationToken cancellationToken = default)
        {
            try
            {
                await _contactRepository.SaveAsync(contact, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log?
            }
        }
    }
}
