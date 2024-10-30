using Application.UseCases.CreateContact.Common;

namespace Shared.Builders;

public class CreateContactRequestBuilder
{
    private readonly CreateContactRequest createContactRequest;

    public CreateContactRequestBuilder() => createContactRequest = CreateDefault();

    private static CreateContactRequest CreateDefault() => new()
    {
        Name = "name",
        Email = "e@mail.com",
        AreaCode = 11,
        Phone = 12345678
    };

    public CreateContactRequestBuilder WithName(string name)
    {
        createContactRequest.Name = name;
        return this;
    }

    public CreateContactRequestBuilder WithEmail(string email)
    {
        createContactRequest.Email = email;
        return this;
    }

    public CreateContactRequestBuilder WithAreaCode(short areaCode)
    {
        createContactRequest.AreaCode = areaCode;
        return this;
    }

    public CreateContactRequestBuilder WithPhone(long phone)
    {
        createContactRequest.Phone = phone;
        return this;
    }

    public CreateContactRequest Build() => createContactRequest;
}
