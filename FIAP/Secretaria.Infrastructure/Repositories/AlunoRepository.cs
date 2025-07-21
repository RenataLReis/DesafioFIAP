using Microsoft.EntityFrameworkCore;
using Secretaria.Domain.Entities;
using Secretaria.Domain.Interfaces;
using Secretaria.Infrastructure.Persistence;

namespace Secretaria.Infrastructure.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly SecretariaDbContext _dbContext;

        public AlunoRepository(SecretariaDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Aluno?> ObterPorNomeAsync(string nome)
        {
            return await _dbContext.Alunos.FirstOrDefaultAsync(a => a.Nome == nome);
        }

        public async Task<Aluno?> ObterPorCpfAsync(string cpf)
        {
            return await _dbContext.Alunos.FirstOrDefaultAsync(a => a.CPF == cpf);
        }

        public async Task<Aluno?> ObterPorIdAsync(int id)
        {
            return await _dbContext.Alunos.FindAsync(id);
        }

        public async Task<IEnumerable<Aluno>> ObterTodosAsync()
        {
            return await _dbContext.Alunos.ToListAsync();
        }

        public async Task CadastrarAsync(Aluno aluno)
        {
            if (aluno == null) throw new ArgumentNullException(nameof(aluno));
            await _dbContext.Alunos.AddAsync(aluno);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Aluno aluno)
        {
            if (aluno == null) throw new ArgumentNullException(nameof(aluno));
            _dbContext.Alunos.Update(aluno);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var aluno = await _dbContext.Alunos.FindAsync(id);
            if (aluno == null) return;
            _dbContext.Alunos.Remove(aluno);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _dbContext.Alunos.AnyAsync(a => a.Id == id);
        }
    }
}
