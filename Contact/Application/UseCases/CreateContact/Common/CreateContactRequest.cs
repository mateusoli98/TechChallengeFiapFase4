using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.CreateContact.Common;

public class CreateContactRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "O DDD é obrigatório.")]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "O DDD deve conter exatamente 2 dígitos.")]
    public required short AreaCode { get; set; }

    [Required(ErrorMessage = "O número de telefone é obrigatório.")]
    [RegularExpression(@"^(9\d{8}|\d{8})$", ErrorMessage = "O telefone deve ter 8 dígitos ou 9 dígitos começando com 9.")]
    public required long Phone { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email não está em um formato válido.")]
    public required string Email { get; set; }
}
