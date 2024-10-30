using Domain.Entities;

namespace Application.UseCases.SearchContact;

public class SearchContactResponse
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required short AreaCode { get; set; }
    public required long Phone { get; set; }
    public string? Email { get; set; }
    public required string State { get; set; }

    public static SearchContactResponse Create(Contact contact) => new()
    {
        Id = contact.Id,
        Name = contact.Name,
        AreaCode = contact.AreaCode,
        Phone = contact.Phone,
        Email = contact.Email,
        State = contact.State
    };
}
