using Secretaria.Domain.Entities;

namespace Secretaria.Domain.Interfaces
{
    public interface IAlunoRepository
    {
        Task<Aluno?> ObterPorNomeAsync(string nome);
        Task<Aluno?> ObterPorCpfAsync(string cpf);
        Task<Aluno?> ObterPorIdAsync(int id);
        Task<IEnumerable<Aluno>> ObterTodosAsync();
        Task CadastrarAsync(Aluno aluno);
        Task AtualizarAsync(Aluno aluno);
        Task RemoverAsync(int id);       
    }
}
