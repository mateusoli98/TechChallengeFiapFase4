using Application.UseCases.UpdateContact.Common;
using Application.UseCases.UpdateContact;
using Application.UseCases.GetContact;
using Infra.Services.Messages;
using Shared.Builders;
using FluentValidation.TestHelper;
using FluentAssertions;
using ErrorOr;
using Moq;

namespace Unit.Application.Usecases.UpdateContact;

public class SendUpdateContactUseCaseTests
{
    private readonly Mock<IGetContactUseCase> _mockGetContactUsecase = new();
    private readonly Mock<IRabbitMqProducerService> mockMessageService = new();
    private readonly SendUpdateContactRequestUseCase _useCase;
    private readonly UpdateContactRequestValidator _validator;

    public SendUpdateContactUseCaseTests()
    {
        _useCase = new SendUpdateContactRequestUseCase(_mockGetContactUsecase.Object, mockMessageService.Object);
        _validator = new UpdateContactRequestValidator();
    }

    [Fact]
    public async Task ShouldRequestUpdateContact_WhenContactFound()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        var request = new UpdateContactRequestBuilder()
            .WithName("Updated Name")
            .WithEmail("updated@email.com")
            .WithAreaCode(11)
            .WithPhone(987654321)
            .Build();

        var existingContact = new GetContactResponseBuilder()
            .WithId(contactId)
            .WithName("Original Name")
            .WithEmail("original@email.com")
            .WithAreaCode(10)
            .WithPhone(123456789)
            .Build();

        _mockGetContactUsecase.Setup(r => r.Execute(contactId, default))
            .ReturnsAsync(existingContact);

        // Act
        var result = await _useCase.Execute(contactId, request);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.Message.Should().Be($"Solicitação de alteração de contato com Id {contactId} realizado com sucesso.");
        mockMessageService.Verify(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnNotFoundError_WhenContactNotFound()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        var request = new UpdateContactRequestBuilder().Build();
        var getUsecaseResponse = Error.NotFound(description: $"Contato com id: {contactId} não encontrado. Revise o Id informado ou tente novamente mais tarde"); ;

        _mockGetContactUsecase.Setup(uc => uc.Execute(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getUsecaseResponse);

        // Act
        var result = await _useCase.Execute(contactId, request);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(Error.Validation().Type);
        result.FirstError.Description.Should().Be($"Contato com id: {contactId} não encontrado. Revise o Id informado ou tente novamente mais tarde");
    }


    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        var request = new UpdateContactRequestBuilder()
            .WithName("")
            .WithAreaCode(11)
            .WithPhone(912345678)
            .Build();

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsTooShort()
    {

        var request = new UpdateContactRequestBuilder()
            .WithName("Jo")
            .WithAreaCode(11)
            .WithPhone(912345678)
            .WithEmail("test@example.com")
            .Build();

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsTooLong()
    {
        var request = new UpdateContactRequestBuilder()
            .WithName(new string('A', 101))
            .WithAreaCode(11)
            .WithPhone(912345678)
            .WithEmail("test@example.com")
            .Build();

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenAreaCodeIsInvalid()
    {
        var request = new UpdateContactRequestBuilder()
           .WithName("Josefina")
           .WithAreaCode(123)
           .WithPhone(912345678)
           .WithEmail("test@example.com")
           .Build();

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.AreaCode);
    }

    [Fact]
    public void ShouldHaveErrorWhenPhoneIsInvalid()
    {
        var request = new UpdateContactRequestBuilder()
           .WithName("Josefina")
           .WithAreaCode(11)
           .WithPhone(1234567)
           .WithEmail("test@example.com")
           .Build();

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void ShouldHaveErrorWhenEmailIsInvalid()
    {
        var request = new UpdateContactRequestBuilder()
           .WithName("Josefina")
           .WithAreaCode(11)
           .WithPhone(912345678)
           .WithEmail("invalid-email")
           .Build();

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var request = new UpdateContactRequestBuilder()
           .WithName("Josefina")
           .WithAreaCode(11)
           .WithPhone(912345678)
           .WithEmail("test@example.com")
           .Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
