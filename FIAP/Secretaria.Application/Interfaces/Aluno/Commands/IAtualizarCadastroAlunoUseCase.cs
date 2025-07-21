using Secretaria.Application.Dtos.Aluno;

namespace Secretaria.Application.Interfaces.Aluno.Commands
{
    public interface IAtualizarCadastroAlunoUseCase
    {
        Task<AlunoDto> ExecuteAsync(AlunoRequestDto aluno);
    }
}
