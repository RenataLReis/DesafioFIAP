using FluentValidation;

namespace Secretaria.Application.Validators
{
    public class SenhaValidator : AbstractValidator<string>
    {
        public SenhaValidator()
        {
            RuleFor(senha => senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
                .Matches("[A-Z]").WithMessage("A senha deve conter ao menos uma letra maiúscula.")
                .Matches("[a-z]").WithMessage("A senha deve conter ao menos uma letra minúscula.")
                .Matches("[0-9]").WithMessage("A senha deve conter ao menos um número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("A senha deve conter ao menos um símbolo especial.");
        }
    }
}
