using FluentValidation;
using Secretaria.Application.Dtos.Aluno;

namespace Secretaria.Application.Validators
{
    public class CadastrarAlunoRequestDtoValidator : AbstractValidator<CadastrarAlunoRequestDto>
    {
        public CadastrarAlunoRequestDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(3).WithMessage("A nome deve ter no mínimo 3 letras.");
    
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.CPF).SetValidator(new CpfValidator());

            RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .Must(data => data <= DateTime.Today).WithMessage("A data de nascimento não pode ser no futuro.");

            RuleFor(x => x.Senha).SetValidator(new PasswordValidator());
        }
    }
}


