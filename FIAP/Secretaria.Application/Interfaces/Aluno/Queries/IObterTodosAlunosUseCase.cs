using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Dtos.Shared;

namespace Secretaria.Application.Interfaces.Aluno.Queries
{
    public interface IObterTodosAlunosUseCase
    {
        Task<ResultadoPaginadoDto<AlunoDto>> ExecuteAsync(int page = 1, int pageSize = 10);
    }
}
