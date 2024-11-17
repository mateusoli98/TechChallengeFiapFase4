using Application.UseCases.DeleteContactPermanently;
using Application.UseCases.DeleteContactPermanently.Interfaces;
using Domain.Repositories.Relational;
using Shared.Builders;
using Moq;
using FluentAssertions;

namespace Unit.Application.Usecases;

public class DeleteContactPermanentlyUseCaseTests
{
    private readonly Mock<IContactRepository> _mockContactRepository = new();
    private readonly IDeleteContactPermanentlyProcessingUseCase _useCase;

    public DeleteContactPermanentlyUseCaseTests()
    {
        _useCase = new DeleteContactPermanentlyProcessingUseCase(_mockContactRepository.Object);
    }

    [Fact]
    public async Task ShouldDeleteContactWhenExists()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        var contact = new ContactBuilder().Build();

        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, It.IsAny<bool>()))
            .ReturnsAsync(contact);

        // Act
        await _useCase.Execute(contactId);

        // Assert
        _mockContactRepository.Verify(repo => repo.PermanentDelete(contact, default), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnErrorWhenContactNotFound()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();

        // Act
        // Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _useCase.Execute(contactId);
        });

        exception.Message.Should().Be("Contato não encontrado.");
    }
}

