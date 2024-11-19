using Application.UseCases.CreateContact;
using Domain.Repositories.Relational;
using Domain.Entities;
using Shared.Builders;
using FluentAssertions;
using Moq;

namespace Unit.Application.Usecases.CreateContact;

public class CreateContactProcessingUseCaseTests
{
    private readonly Mock<IContactRepository> mockContactRepository = new();
    private readonly CreateContactProcessingUseCase _usecase;

    public CreateContactProcessingUseCaseTests()
    {
        _usecase = new(mockContactRepository.Object);
    }

    [Fact]
    public async Task ShouldProcessWithSuccess()
    {
        var contactId = Guid.NewGuid().ToString();
        var contact = new ContactBuilder().WithId(contactId).Build();

        mockContactRepository.Setup(r => r.Exists(It.IsAny<short>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _usecase.Execute(contact);

        mockContactRepository.Verify(r => r.SaveAsync(It.Is<Contact>(c =>
            c.Id == contactId
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldFail_WhenPhoneNumberAlreadyExists()
    {
        var contactId = Guid.NewGuid().ToString();
        var contact = new ContactBuilder().WithId(contactId).Build();

        mockContactRepository.Setup(r => r.Exists(It.IsAny<short>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _usecase.Execute(contact);
        });

        exception.Message.Should().Be("Telefone já cadastrado anteriormente no sistema.");
    }
}
