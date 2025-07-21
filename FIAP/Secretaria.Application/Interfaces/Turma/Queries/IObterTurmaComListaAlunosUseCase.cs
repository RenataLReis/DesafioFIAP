using Secretaria.Application.Dtos.Turma;

namespace Secretaria.Application.Interfaces.Turma.Queries
{
    public interface IObterTurmaComListaAlunosUseCase
    {
        Task<TurmaDto> ExecuteAsync(int turmaId, int page = 1, int pageSize = 10);
    }
}
