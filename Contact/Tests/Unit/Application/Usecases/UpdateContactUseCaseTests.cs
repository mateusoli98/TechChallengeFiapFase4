//using Application.UseCases.UpdateContact;
//using Application.UseCases.UpdateContact.Common;
//using Application.UseCases.UpdateContact.Interfaces;
//using Domain.Repositories.Relational;
//using ErrorOr;
//using FluentAssertions;
//using FluentValidation.TestHelper;
//using Moq;
//using Shared.Builders;

//namespace Unit.Application.Usecases;

//public class UpdateContactUseCaseTests
//{
//    private readonly Mock<IContactRepository> _mockContactRepository = new();
//    private readonly ISendUpdateContactRequestUseCase _useCase;
//    private readonly UpdateContactRequestValidator _validator;

//    public UpdateContactUseCaseTests()
//    {
//        _useCase = new SendUpdateContactRequestUseCase(_mockContactRepository.Object);
//        _validator = new UpdateContactRequestValidator();
//    }

//    [Fact]
//    public async Task ShouldUpdateContact_WhenContactFound()
//    {
//        // Arrange
//        var contactId = 1;
//        var request = new UpdateContactRequestBuilder()
//            .WithName("Updated Name")
//            .WithEmail("updated@email.com")
//            .WithAreaCode(11)
//            .WithPhone(987654321)
//            .Build();

//        var existingContact = new ContactBuilder()
//            .WithId(contactId)
//            .WithName("Original Name")
//            .WithEmail("original@email.com")
//            .WithAreaCode(10)
//            .WithPhone(123456789)
//            .Build();

//        _mockContactRepository.Setup(r => r.GetByIdAsync(contactId, default, true))
//            .ReturnsAsync(existingContact);

//        // Act
//        var result = await _useCase.Execute(contactId, request);

//        // Assert
//        result.Should().NotBeNull();
//        result.Value.Should().NotBeNull();
//        result.IsError.Should().BeFalse();
//        result.Value.Id.Should().Be(contactId);
//        result.Value.Name.Should().Be(request.Name);
//        result.Value.Email.Should().Be(request.Email);
//        result.Value.AreaCode.Should().Be(request.AreaCode);
//        result.Value.Phone.Should().Be(request.Phone);
//    }

//    [Fact]
//    public async Task ShouldReturnNotFoundError_WhenContactNotFound()
//    {
//        // Arrange
//        var contactId = 1;
//        var request = new UpdateContactRequestBuilder().Build();

//        // Act
//        var result = await _useCase.Execute(contactId, request);

//        // Assert
//        result.IsError.Should().BeTrue();
//        result.FirstError.Type.Should().Be(Error.NotFound().Type);
//    }


//    [Fact]
//    public void ShouldHaveErrorWhenNameIsEmpty()
//    {
//        var request = new UpdateContactRequestBuilder()
//            .WithName("")
//            .WithAreaCode(11)
//            .WithPhone(912345678)
//            .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldHaveValidationErrorFor(x => x.Name);
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenNameIsTooShort()
//    {

//        var request = new UpdateContactRequestBuilder()
//            .WithName("Jo")
//            .WithAreaCode(11)
//            .WithPhone(912345678)
//            .WithEmail("test@example.com")
//            .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldHaveValidationErrorFor(x => x.Name);
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenNameIsTooLong()
//    {
//        var request = new UpdateContactRequestBuilder()
//            .WithName(new string('A', 101))
//            .WithAreaCode(11)
//            .WithPhone(912345678)
//            .WithEmail("test@example.com")
//            .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldHaveValidationErrorFor(x => x.Name);
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenAreaCodeIsInvalid()
//    {
//        var request = new UpdateContactRequestBuilder()
//           .WithName("Josefina")
//           .WithAreaCode(123)
//           .WithPhone(912345678)
//           .WithEmail("test@example.com")
//           .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldHaveValidationErrorFor(x => x.AreaCode);
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenPhoneIsInvalid()
//    {
//        var request = new UpdateContactRequestBuilder()
//           .WithName("Josefina")
//           .WithAreaCode(11)
//           .WithPhone(1234567)
//           .WithEmail("test@example.com")
//           .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldHaveValidationErrorFor(x => x.Phone);
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenEmailIsInvalid()
//    {
//        var request = new UpdateContactRequestBuilder()
//           .WithName("Josefina")
//           .WithAreaCode(11)
//           .WithPhone(912345678)
//           .WithEmail("invalid-email")
//           .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldHaveValidationErrorFor(x => x.Email);
//    }

//    [Fact]
//    public void ShouldNotHaveErrorWhenModelIsValid()
//    {
//        var request = new UpdateContactRequestBuilder()
//           .WithName("Josefina")
//           .WithAreaCode(11)
//           .WithPhone(912345678)
//           .WithEmail("test@example.com")
//           .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldNotHaveAnyValidationErrors();
//    }
//}
