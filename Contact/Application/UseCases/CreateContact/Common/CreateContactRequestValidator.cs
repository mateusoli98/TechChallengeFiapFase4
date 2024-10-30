using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.UseCases.CreateContact.Common
{
    public class CreateContactRequestValidator : AbstractValidator<CreateContactRequest>
    {
        public CreateContactRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

            RuleFor(x => x.AreaCode)
                .NotEmpty().WithMessage("O DDD é obrigatório.")
                .Must(areaCode => Regex.IsMatch(areaCode.ToString(), @"^\d{2}$"))
                .WithMessage("O DDD deve conter exatamente 2 dígitos.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O número de telefone é obrigatório.")
                .Must(phone => Regex.IsMatch(phone.ToString(), @"^(9\d{8}|\d{8})$"))
                .WithMessage("O telefone deve ter 8 dígitos ou 9 dígitos começando com 9.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email não está em um formato válido.");
        }
    }
}
