using Secretaria.Domain.Entities;

namespace Secretaria.Domain.Interfaces
{
    public interface ITurmaRepository
    {
        Task<Turma?> ObterPorNomeAsync(string nome);
        Task<Turma?> ObterPorIdAsync(int id);
        Task<IEnumerable<Turma>> ObterTodasAsync();
        Task<Turma> CadastrarAsync(Turma turma);
        Task AtualizarAsync(Turma turma);
        Task RemoverAsync(int id);
        Task<bool> ExisteAsync(int id);       
    }
}
