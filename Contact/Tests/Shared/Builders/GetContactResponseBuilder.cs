using Application.UseCases.GetContact;
using CrossCutting.Utils;

namespace Shared.Builders;

public class GetContactResponseBuilder
{
    private readonly GetContactResponse _contact;

    public GetContactResponseBuilder()
    {
        _contact = CreateDefault();
    }

    private static GetContactResponse CreateDefault() => new()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "John Doe",
        Email = "johndoe@example.com",
        AreaCode = 11,
        Phone = 123456789,
        State = AreaCodeDictionary.GetStateByAreaCode(11),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    public GetContactResponseBuilder WithId(string id)
    {
        _contact.Id = id;
        return this;
    }

    public GetContactResponseBuilder WithName(string name)
    {
        _contact.Name = name;
        return this;
    }

    public GetContactResponseBuilder WithEmail(string email)
    {
        _contact.Email = email;
        return this;
    }

    public GetContactResponseBuilder WithAreaCode(short areaCode)
    {
        _contact.AreaCode = areaCode;
        _contact.State = AreaCodeDictionary.GetStateByAreaCode(areaCode);
        return this;
    }

    public GetContactResponseBuilder WithPhone(long phone)
    {
        _contact.Phone = phone;
        return this;
    }

    public GetContactResponseBuilder WithCreatedAt(DateTime createdAt)
    {
        _contact.CreatedAt = createdAt;
        return this;
    }

    public GetContactResponseBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _contact.UpdatedAt = updatedAt;
        return this;
    }

    public GetContactResponse Build() => _contact;
}
