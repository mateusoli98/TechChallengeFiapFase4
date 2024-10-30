//using Application.UseCases.DeleteContact;
//using Application.UseCases.DeleteContact.Interfaces;
//using Domain.Entities;
//using Domain.Repositories.Relational;
//using ErrorOr;
//using FluentAssertions;
//using Moq;
//using Shared.Builders;

//namespace Unit.Application.Usecases;

//public class DeleteContactUseCaseTests
//{
//    private readonly Mock<IContactRepository> _mockContactRepository = new();
//    private readonly IDeleteContactProcessingUseCase _useCase;

//    public DeleteContactUseCaseTests()
//    {
//        _useCase = new SendDeleteContactRequestUseCase(_mockContactRepository.Object);
//    }

//    [Fact]
//    public async Task ShouldDeleteContactWhenExists()
//    {
//        // Arrange
//        int contactId = 1;
//        var contact = new ContactBuilder().Build();

//        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, true))
//            .ReturnsAsync(contact);

//        // Act
//        var result = await _useCase.Execute(contactId);

//        // Assert
//        result.Should().BeNull();
//        _mockContactRepository.Verify(repo => repo.DeleteAsync(contact, default), Times.Once);
//    }

//    [Fact]
//    public async Task ShouldReturnErrorWhenContactNotFound()
//    {
//        // Arrange
//        int contactId = 1;

//        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, true))
//            .ReturnsAsync(() => null);

//        // Act
//        var result = await _useCase.Execute(contactId);

//        // Assert
//        result.Should().NotBeNull()
//            .And.BeOfType<Error>();

//        _mockContactRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Contact>(), default), Times.Never);
//    }
//}

