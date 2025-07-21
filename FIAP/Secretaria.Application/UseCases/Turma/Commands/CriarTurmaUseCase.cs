using Secretaria.Application.Dtos.Turma;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Turma.Commands
{
    public class CriarTurmaUseCase
    {
        public readonly TurmaRequestDto _turmaDto;
        public readonly ITurmaRepository _turmaRepository;
        public CriarTurmaUseCase(TurmaRequestDto turmaDto, ITurmaRepository turmaRepository)
        {
            _turmaDto = turmaDto ?? throw new ArgumentNullException(nameof(turmaDto));
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        public async Task<TurmaDto> ExecuteAsync(TurmaRequestDto turmaDto)
        {
            //cancellationId não é necessário por conta das operações assíncronas que virão?
            //chamar validator
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
