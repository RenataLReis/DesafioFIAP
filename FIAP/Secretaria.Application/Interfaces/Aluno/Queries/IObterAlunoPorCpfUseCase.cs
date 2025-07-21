namespace Secretaria.Application.Interfaces.Aluno.Queries
{
    public interface IObterAlunoPorCpfUseCase
    {
        Task<Domain.Entities.Aluno?> ExecuteAsync(string cpf);
    }
}
