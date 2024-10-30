//using Application.UseCases.SearchContact;
//using Domain.DomainObjects.Filters;
//using Domain.Entities;
//using Domain.Repositories.Relational;
//using FluentAssertions;
//using Moq;
//using Shared.Builders;

//namespace Unit.Application.Usecases;

//public class SearchContactUseCaseTests
//{
//    private readonly Mock<IContactRepository> _mockContactRepository = new();
//    private readonly ISearchContactUseCase _useCase;

//    public SearchContactUseCaseTests()
//    {
//        _useCase = new SearchContactUseCase(_mockContactRepository.Object);
//    }

//    [Fact]
//    public async Task ShouldReturnPaginationResultWithSearchContactResponses_WhenContactsFound()
//    {
//        // Arrange
//        var filter = new ContactFilterBuilder().Build();
//        var contacts = new List<Contact>
//        {
//            new ContactBuilder().WithId(1).Build(),
//            new ContactBuilder().WithId(2).Build()
//        };

//        var expectedResult = new PaginationResult<Contact>(
//            total: contacts.Count,
//            items: contacts
//        );

//        _mockContactRepository.Setup(r => r.SearchAsync(It.IsAny<ContactFilter>(), default))
//            .ReturnsAsync(expectedResult);

//        // Act
//        var result = await _useCase.Execute(filter, CancellationToken.None);

//        // Assert
//        result.Value.Should().NotBeNull();
//        result.IsError.Should().BeFalse();
//        result.Value.Total.Should().Be(contacts.Count);
//        result.Value.Items.Should().BeEquivalentTo(contacts.Select(c => SearchContactResponse.Create(c)));
//    }

//    [Fact]
//    public async Task ShouldReturnEmptyPaginationResult_WhenNoContactsFound()
//    {
//        // Arrange
//        var filter = new ContactFilterBuilder().Build();
//        var expectedResult = new PaginationResult<Contact>(
//            total: 0,
//            items: Enumerable.Empty<Contact>()
//        );

//        _mockContactRepository.Setup(r => r.SearchAsync(It.IsAny<ContactFilter>(), default))
//            .ReturnsAsync(expectedResult);

//        // Act
//        var result = await _useCase.Execute(filter, CancellationToken.None);

//        // Assert
//        result.Value.Should().NotBeNull();
//        result.Value.Total.Should().Be(0);
//        result.Value.Items.Should().BeEmpty();
//        result.IsError.Should().BeFalse();
//    }
//}