using Application.UseCases.DeleteContact.Interfaces;
using Application.UseCases.DeleteContact;
using Domain.Repositories.Relational;
using Domain.Entities;
using Shared.Builders;
using FluentAssertions;
using ErrorOr;
using Moq;

namespace Unit.Application.Usecases;

public class DeleteContactUseCaseTests
{
    private readonly Mock<IContactRepository> _mockContactRepository = new();
    private readonly IDeleteContactProcessingUseCase _useCase;

    public DeleteContactUseCaseTests()
    {
        _useCase = new DeleteContactProcessingUseCase(_mockContactRepository.Object);
    }

    [Fact]
    public async Task ShouldDeleteContactWhenExists()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        var contact = new ContactBuilder().Build();

        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, true))
            .ReturnsAsync(contact);

        // Act
        var result = await _useCase.Execute(contactId);

        // Assert
        result.Should().BeNull();
        _mockContactRepository.Verify(repo => repo.DeleteAsync(contact, default), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnErrorWhenContactNotFound()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();

        // Act
        var result = await _useCase.Execute(contactId);

        // Assert
        result.Should().NotBeNull()
            .And.BeOfType<Error>();

        _mockContactRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Contact>(), default), Times.Never);
    }
}

