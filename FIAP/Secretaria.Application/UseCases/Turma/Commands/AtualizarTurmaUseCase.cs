using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Turma.Commands
{
    public class AtualizarTurmaUseCase : IAtualizarTurmaUseCase
    {
        private readonly ITurmaRepository _turmaRepository;

        public AtualizarTurmaUseCase(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        public async Task<TurmaDto> ExecuteAsync(TurmaRequestDto request)
        {
            var turma = await _turmaRepository.ObterPorNomeAsync(request.Nome);

            if (turma == null)          
                throw new InvalidOperationException($"Turma com nome '{request.Nome}' não encontrada.");

            var turmaAtualizada = Domain.Entities.Turma.Criar(
                request.Nome,
                request.Descricao);

            await _turmaRepository.AtualizarAsync(turmaAtualizada);

            return new TurmaDto
            {
                Id = turmaAtualizada.Id,
                Nome = turmaAtualizada.Nome,
                Descricao = turmaAtualizada.Descricao ?? string.Empty
            };
        }
    }
}
