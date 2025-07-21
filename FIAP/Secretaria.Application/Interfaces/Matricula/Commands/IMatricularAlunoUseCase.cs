using Secretaria.Application.Dtos.Matricula;

namespace Secretaria.Application.Interfaces.Matricula.Commands
{
    public interface IMatricularAlunoUseCase
    {
        Task<MatriculaDto> ExecuteAsync(MatricularAlunoRequestDto requestDto);
    }
}
