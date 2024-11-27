using Application.UseCases.CreateContact.Interfaces;
using Domain.Repositories.Relational;
using Domain.Entities;

namespace Application.UseCases.CreateContact
{
    public class CreateContactProcessingUseCase(IContactRepository repository) : ICreateContactProcessingUseCase
    {
        private readonly IContactRepository _contactRepository = repository;

        public async Task Execute(Contact contact, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Validando se usuário já existe");
            var alreadyExists = await _contactRepository.Exists(contact.AreaCode, contact.Phone, cancellationToken);
            if (!alreadyExists)
            {
                Console.WriteLine($"Salvando usuário '{contact.Id}'");
                await _contactRepository.SaveAsync(contact, cancellationToken);
                return;
            }

            Console.WriteLine("Erro: usuário já existe no banco de dados");
            throw new Exception("Telefone já cadastrado anteriormente no sistema.");
        }
    }
}
