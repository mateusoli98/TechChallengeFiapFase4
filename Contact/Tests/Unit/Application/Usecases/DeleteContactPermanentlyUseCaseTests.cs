//using Application.UseCases.DeleteContactPermanently;
//using Application.UseCases.DeleteContactPermanently.Interfaces;
//using Domain.Entities;
//using Domain.Repositories.Relational;
//using ErrorOr;
//using FluentAssertions;
//using Moq;
//using Shared.Builders;

//namespace Unit.Application.Usecases;

//public class DeleteContactPermanentlyUseCaseTests
//{
//    private readonly Mock<IContactRepository> _mockContactRepository = new();
//    private readonly ISendDeleteContactPermanentlyRequestUseCase _useCase;

//    public DeleteContactPermanentlyUseCaseTests()
//    {
//        _useCase = new DeleteContactPermanentlyProcessingUseCase(_mockContactRepository.Object);
//    }

//    [Fact]
//    public async Task ShouldDeleteContactWhenExists()
//    {
//        // Arrange
//        int contactId = 1;
//        var contact = new ContactBuilder().Build();

//        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, It.IsAny<bool>()))
//            .ReturnsAsync(contact);

//        // Act
//        var result = await _useCase.Execute(contactId);

//        // Assert
//        result.Should().BeNull();
//        _mockContactRepository.Verify(repo => repo.PermanentDelete(contact, default), Times.Once);
//    }

//    [Fact]
//    public async Task ShouldReturnErrorWhenContactNotFound()
//    {
//        // Arrange
//        int contactId = 1;

//        _mockContactRepository.Setup(repo => repo.GetByIdAsync(contactId, default, false))
//            .ReturnsAsync(() => null);

//        // Act
//        var result = await _useCase.Execute(contactId);

//        // Assert
//        result.Should().NotBeNull()
//            .And.BeOfType<Error>();

//        _mockContactRepository.Verify(repo => repo.PermanentDelete(It.IsAny<Contact>(), default), Times.Never);
//    }
//}

