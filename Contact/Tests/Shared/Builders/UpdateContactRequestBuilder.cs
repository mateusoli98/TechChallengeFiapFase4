using Application.UseCases.UpdateContact.Common;

namespace Shared.Builders;

public class UpdateContactRequestBuilder
{
    private readonly UpdateContactRequest updateContactRequest;

    public UpdateContactRequestBuilder() => updateContactRequest = CreateDefault();

    private static UpdateContactRequest CreateDefault() => new()
    {
        Name = "Another Name",
        Email = "another_e@mail.com",
        AreaCode = 21,
        Phone = 987654321
    };

    public UpdateContactRequestBuilder WithName(string name)
    {
        updateContactRequest.Name = name;
        return this;
    }

    public UpdateContactRequestBuilder WithEmail(string email)
    {
        updateContactRequest.Email = email;
        return this;
    }

    public UpdateContactRequestBuilder WithAreaCode(short areaCode)
    {
        updateContactRequest.AreaCode = areaCode;
        return this;
    }

    public UpdateContactRequestBuilder WithPhone(long phone)
    {
        updateContactRequest.Phone = phone;
        return this;
    }

    public UpdateContactRequest Build() => updateContactRequest;
}
