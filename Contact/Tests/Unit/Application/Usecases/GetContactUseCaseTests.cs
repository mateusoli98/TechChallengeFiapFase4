using Application.UseCases.GetContact;
using Domain.Entities;
using Domain.Repositories.Relational;
using ErrorOr;
using FluentAssertions;
using Moq;
using Shared.Builders;

namespace Unit.Application.Usecases;

public class GetContactUseCaseTests
{
    private readonly Mock<IContactRepository> _mockContactRepository = new();
    private readonly IGetContactUseCase _useCase;

    public GetContactUseCaseTests()
    {
        _useCase = new GetContactUseCase(_mockContactRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnContactResponseWhenContactExists()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        var contact = new ContactBuilder().WithId(contactId).Build();
        var expectedResponse = new GetContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            AreaCode = contact.AreaCode,
            Phone = contact.Phone,
            Email = contact.Email,
            State = contact.State,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };

        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, true))
            .ReturnsAsync(contact);

        // Act
        var result = await _useCase.Execute(contactId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ErrorOr<GetContactResponse>>()
              .Which.IsError.Should().BeFalse();

        result.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task ShouldReturnNotFoundErrorWhenContactNotFound()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        Contact? contact = null;

        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, true))
            .ReturnsAsync(contact);

        // Act
        var result = await _useCase.Execute(contactId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ErrorOr<GetContactResponse>>()
              .Which.IsError.Should().BeTrue();

        result.FirstError.Type.Should().Be(Error.NotFound().Type);
    }
}