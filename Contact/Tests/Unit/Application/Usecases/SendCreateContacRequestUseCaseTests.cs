//using Application.UseCases.CreateContact;
//using Application.UseCases.CreateContact.Common;
//using Application.UseCases.CreateContact.Interfaces;
//using Domain.Entities;
//using Domain.Repositories.Relational;
//using FluentAssertions;
//using FluentValidation.TestHelper;
//using Moq;
//using Shared.Builders;

//namespace Unit.Application.Usecases;

//public class SendCreateContacRequestUseCaseTests
//{
//    public readonly Mock<IContactRepository> mockContactResitory = new();
//    public readonly ISendCreateContactRequestUseCase useCase;
//    private readonly CreateContactRequestValidator _validator;

//    public SendCreateContacRequestUseCaseTests()
//    {
//        useCase = new SendCreateContactRequestUseCase();
//        _validator = new CreateContactRequestValidator();
//    }

//    [Fact]
//    public async Task ShouldReturnId_WhenContactWasCreatedWithSucessuful()
//    {
//        var request = new CreateContactRequestBuilder().Build();

//        var result = await useCase.Execute(request);

//        result.Value.Id.Should().NotBeNull();
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenNameIsEmpty()
//    {
//        var request = new CreateContactRequestBuilder()
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
//        var model = new CreateContactRequest { Name = "Jo", AreaCode = 11, Phone = 912345678, Email = "test@example.com" };
//        var request = new CreateContactRequestBuilder()
//            .WithName("Jo")
//            .WithAreaCode(11)
//            .WithPhone(912345678)
//            .WithEmail("test@example.com")
//            .Build();

//        var result = _validator.TestValidate(model);

//        result.ShouldHaveValidationErrorFor(x => x.Name);
//    }

//    [Fact]
//    public void ShouldHaveErrorWhenNameIsTooLong()
//    {
//        var request = new CreateContactRequestBuilder()
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
//        var request = new CreateContactRequestBuilder()
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
//        var request = new CreateContactRequestBuilder()
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
//        var request = new CreateContactRequestBuilder()
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
//        var request = new CreateContactRequestBuilder()
//           .WithName("Josefina")
//           .WithAreaCode(11)
//           .WithPhone(912345678)
//           .WithEmail("test@example.com")
//           .Build();

//        var result = _validator.TestValidate(request);

//        result.ShouldNotHaveAnyValidationErrors();
//    }
//}
