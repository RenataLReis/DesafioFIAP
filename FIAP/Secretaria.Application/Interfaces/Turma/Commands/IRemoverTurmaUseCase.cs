namespace Secretaria.Application.Interfaces.Turma.Commands
{
    public interface IRemoverTurmaUseCase
    {
        Task ExecuteAsync(int id);
    }
}
