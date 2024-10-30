using Domain.Entities;

namespace Application.UseCases.GetContact;

public class GetContactResponse
{
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required short AreaCode { get; set; }
    public required long Phone { get; set; }
    public string? Email { get; set; }
    public required string State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static GetContactResponse Create(Contact contact) => new()
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

    public static Contact GetContact(GetContactResponse contact) => new()
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
}
