using Secretaria.Application.Interfaces.Turma.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Turma.Commands
{
    public class RemoverTurmaUseCase : IRemoverTurmaUseCase
    {
        private readonly ITurmaRepository _turmaRepository;

        public RemoverTurmaUseCase(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            var turma = await _turmaRepository.ObterPorIdAsync(id);

            if (turma == null)
                throw new InvalidOperationException($"Turma com ID '{id}' não encontrada.");

            await _turmaRepository.RemoverAsync(id);
        }
    }
}
