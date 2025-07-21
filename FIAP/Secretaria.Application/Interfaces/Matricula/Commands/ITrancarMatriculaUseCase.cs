namespace Secretaria.Application.Interfaces.Matricula.Commands
{
    public interface ITrancarMatriculaUseCase
    {
        Task ExecuteAsync(int id);
    }
}
