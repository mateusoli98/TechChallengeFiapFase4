using Application.UseCases.UpdateContact;
using Application.UseCases.UpdateContact.Common;
using Domain.Entities;
using Domain.Repositories.Relational;
using FluentAssertions;
using Moq;
using Shared.Builders;

namespace Unit.Application.Usecases.UpdateContact;

public class UpdateContactProcessingUseCaseTests
{ 
    private readonly Mock<IContactRepository> mockContactRepository = new();
    private readonly UpdateContactProcessingUseCase _useCase;

    public UpdateContactProcessingUseCaseTests()
    {
        _useCase = new UpdateContactProcessingUseCase(mockContactRepository.Object);
    }

    [Fact]
    public async Task ShouldProcessUpdateWithSuccess()
    {
        var contactId = Guid.NewGuid().ToString();
        var oldContact = new ContactBuilder().WithId(contactId).Build();
        var newContact = new ContactBuilder()
            .WithId(contactId)
            .WithName("New Name")
            .WithEmail("other@mail.com")
            .WithAreaCode(12)
            .WithPhone(987654321)
            .Build();

        mockContactRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
            .ReturnsAsync(oldContact);

        mockContactRepository.Setup(r => r.Exists(It.IsAny<short>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var exception = await Record.ExceptionAsync(async () => await _useCase.Execute(newContact));

        exception.Should().BeNull();
        mockContactRepository.Verify(r => r.UpdateAsync(It.Is<Contact>(c =>
              c.Name == newContact.Name &
              c.Email == newContact.Email &
              c.AreaCode == newContact.AreaCode &
              c.Phone == newContact.Phone              
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowException_WhenContactIsNotFound()
    {
        var contactId = Guid.NewGuid().ToString();

        var newContact = new ContactBuilder()
            .WithId(contactId)
            .WithName("New Name")
            .WithEmail("other@mail.com")
            .WithAreaCode(12)
            .WithPhone(987654321)
            .Build();

        var exception = await Record.ExceptionAsync(async () => await _useCase.Execute(newContact));

        exception.Message.Should().Be("Contato não encontrado.");
        mockContactRepository.Verify(r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldThrowException_WhenCompletePhoneNumberAlreadyExists()
    {
        var contactId = Guid.NewGuid().ToString();
        var oldContact = new ContactBuilder().WithId(contactId).Build();
        var newContact = new ContactBuilder()
            .WithId(contactId)
            .WithName("New Name")
            .WithEmail("other@mail.com")
            .Build();

        mockContactRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
            .ReturnsAsync(oldContact);

        mockContactRepository.Setup(r => r.Exists(It.IsAny<short>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Record.ExceptionAsync(async () => await _useCase.Execute(newContact));

        exception.Message.Should().Be("DDD + Telefone informado já está cadastrado no sistema.");
        mockContactRepository.Verify(r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
