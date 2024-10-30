using CrossCutting.Utils;
using Domain.Entities;

namespace Shared.Builders;

public class ContactBuilder
{
    private readonly Contact _contact;

    public ContactBuilder()
    {
        _contact = CreateDefault();
    }

    private static Contact CreateDefault() => new()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "John Doe",
        Email = "johndoe@example.com",
        AreaCode = 11,
        Phone = 123456789,
        State = AreaCodeDictionary.GetStateByAreaCode(11),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        IsEnabled = true
    };

    public ContactBuilder WithId(string id)
    {
        _contact.Id = id;
        return this;
    }

    public ContactBuilder WithName(string name)
    {
        _contact.Name = name;
        return this;
    }

    public ContactBuilder WithEmail(string email)
    {
        _contact.Email = email;
        return this;
    }

    public ContactBuilder WithAreaCode(short areaCode)
    {
        _contact.AreaCode = areaCode;
        _contact.State = AreaCodeDictionary.GetStateByAreaCode(areaCode);
        return this;
    }

    public ContactBuilder WithPhone(long phone)
    {
        _contact.Phone = phone;
        return this;
    }

    public ContactBuilder WithCreatedAt(DateTime createdAt)
    {
        _contact.CreatedAt = createdAt;
        return this;
    }

    public ContactBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _contact.UpdatedAt = updatedAt;
        return this;
    }

    public ContactBuilder WithIsEnabled(bool isEnabled)
    {
        _contact.IsEnabled = isEnabled;
        return this;
    }

    public Contact Build() => _contact;
}
