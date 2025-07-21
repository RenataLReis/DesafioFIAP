using FluentValidation;
using Secretaria.Application.Dtos.Turma;

namespace Secretaria.Application.Validators
{
    public class TurmaRequestDtoValidator : AbstractValidator<TurmaRequestDto>
    {
        public TurmaRequestDtoValidator()
        {
            RuleFor(turma => turma.Nome)
                .NotEmpty().WithMessage("O nome da turma é obrigatório.")
                .MinimumLength(3).WithMessage("O nome da turma deve ter no mínimo 3 caracteres.");

            RuleFor(turma => turma.Descricao)
                .MaximumLength(500).WithMessage("A descrição da turma não pode exceder 500 caracteres.");
        }
    }
}
