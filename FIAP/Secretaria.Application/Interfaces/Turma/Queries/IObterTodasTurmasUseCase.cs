using Secretaria.Application.Dtos.Shared;
using Secretaria.Application.Dtos.Turma;

namespace Secretaria.Application.Interfaces.Turma.Queries
{
    public interface IObterTodasTurmasUseCase
    {
        Task<ResultadoPaginadoDto<TurmaDto>> ExecuteAsync(int page = 1, int pageSize = 10);
    }
}
