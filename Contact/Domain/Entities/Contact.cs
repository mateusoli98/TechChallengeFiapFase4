namespace Domain.Entities;

public class Contact
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required short AreaCode { get; set; }
    public required long Phone { get; set; }
    public required string Email { get; set; }
    public required string State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsEnabled { get; set; }
}
