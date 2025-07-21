namespace Secretaria.Application.Interfaces.Aluno.Commands
{
    public interface IRemoverAlunoUseCase
    {
        Task ExecuteAsync(int id);
    }
}
