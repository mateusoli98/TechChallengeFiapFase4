using Application.UseCases.DeleteContactPermanently;
using Application.UseCases.GetContact;
using ErrorOr;
using FluentAssertions;
using Infra.Services.Messages;
using Moq;
using Shared.Builders;

namespace Unit.Application.Usecases.DeleteContactPermanently;

public class SendDeleteContactPermanentlyRequestUseCaseTests
{
    private readonly Mock<IGetContactUseCase> mockGetContactUsecase = new();
    private readonly Mock<IRabbitMqProducerService> mockMessageService = new();
    private readonly SendDeleteContactPermanentlyRequestUseCase _usecase;

    public SendDeleteContactPermanentlyRequestUseCaseTests()
    {
        _usecase = new(mockGetContactUsecase.Object, mockMessageService.Object);
    }

    [Fact]
    public async Task ShouldSendRequestWithSuccess()
    {
        var contactId = Guid.NewGuid().ToString();
        var contact = new ContactBuilder().WithId(contactId).Build();
        var getContactResponse = new GetContactResponse
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

        mockGetContactUsecase.Setup(uc => uc.Execute(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getContactResponse);

        var result = await _usecase.Execute(contactId, default);

        result.Should().BeNull();
        mockMessageService.Verify(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnError_WhenContactIsNotFound()
    {
        var contactId = Guid.NewGuid().ToString();
        var getContactResponse = Error.NotFound();

        mockGetContactUsecase.Setup(uc => uc.Execute(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getContactResponse);

        var result = await _usecase.Execute(contactId, default);

        result.Value.Description.Should().Be($"Contato com Id {contactId} não encontrado. Revise o Id informado ou tente novamente mais tarde");
        mockMessageService.Verify(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
