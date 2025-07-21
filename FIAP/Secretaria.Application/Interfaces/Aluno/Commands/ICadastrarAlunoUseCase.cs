using Secretaria.Application.Dtos.Aluno;

namespace Secretaria.Application.Interfaces.Aluno.Commands
{
    public interface ICadastrarAlunoUseCase
    {
        Task ExecuteAsync(AlunoRequestDto request);
    }
}
