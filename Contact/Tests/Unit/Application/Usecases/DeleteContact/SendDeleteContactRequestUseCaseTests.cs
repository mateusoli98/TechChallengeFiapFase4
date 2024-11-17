using Application.UseCases.DeleteContact;
using Application.UseCases.GetContact;
using Domain.Entities;
using ErrorOr;
using FluentAssertions;
using Infra.Services.Messages;
using Moq;
using Shared.Builders;

namespace Unit.Application.Usecases.DeleteContact;

public class SendDeleteContactRequestUseCaseTests
{
    private readonly Mock<IGetContactUseCase> mockGetContactUsecase = new();
    private readonly Mock<IRabbitMqProducerService> mockMessageService = new();
    private readonly SendDeleteContactRequestUseCase _usecase;

    public SendDeleteContactRequestUseCaseTests()
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

        var result = await _usecase.Execute(contactId);

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

        var result = await _usecase.Execute(contactId);

        result.Value.Description.Should().Be($"Contato com Id {contactId} não encontrado. Revise o Id informado ou tente novamente mais tarde");
        mockMessageService.Verify(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
