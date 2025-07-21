using Secretaria.Domain.Entities;

namespace Secretaria.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task CriarAsync(Matricula matricula);
        Task AtualizarAsync(Matricula matricula);
        Task RemoverAsync(int id);
        Task<Matricula?> ObterPorIdAsync(int id);
        Task<IEnumerable<Matricula>> ObterTodasAsync();
        Task<IEnumerable<Matricula>> ObterPorAlunoIdAsync(int alunoId);
        Task<IEnumerable<Matricula>> ObterPorTurmaIdAsync(int turmaId);
        Task<string?> ObterUltimoNumeroAsync(string ano);
    }
}
