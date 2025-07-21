namespace Secretaria.Application.Interfaces.Aluno.Queries
{
    public interface IObterAlunoPorNomeUseCase
    {
        Task<Domain.Entities.Aluno?> ExecuteAsync(string nome);
    }
}
