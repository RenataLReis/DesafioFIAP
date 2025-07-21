using FluentValidation;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Turma.Commands
{
    public class CriarTurmaUseCase : ICriarTurmaUseCase
    {
        public readonly ITurmaRepository _turmaRepository;
        public readonly IValidator<string> _validator;
        public CriarTurmaUseCase(ITurmaRepository turmaRepository, IValidator<string> validator)
        {
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<TurmaDto> ExecuteAsync(TurmaRequestDto turmaDto)
        {
            if (await _turmaRepository.ObterPorNomeAsync(turmaDto.Nome) != null)
            {
                throw new InvalidOperationException($"O curso '{turmaDto.Nome}' já está cadastrado.");
            }

            var turma = Domain.Entities.Turma.Criar(
                turmaDto.Nome,
                turmaDto.Descricao);

            var turmaCriada = await _turmaRepository.CadastrarAsync(turma);

            return new TurmaDto
            {
                Id = turmaCriada.Id,
                Nome = turmaCriada.Nome,
                Descricao = turmaCriada.Descricao ?? string.Empty
            };
        }
    }
}
