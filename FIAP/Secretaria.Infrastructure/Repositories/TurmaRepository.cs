using Microsoft.EntityFrameworkCore;
using Secretaria.Domain.Entities;
using Secretaria.Domain.Interfaces;
using Secretaria.Infrastructure.Persistence;

namespace Secretaria.Infrastructure.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        public readonly SecretariaDbContext _dbContext;

        public TurmaRepository(SecretariaDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Turma?> ObterPorNomeAsync(string nome)
        {
            return await _dbContext.Turmas
                .Include(t => t.Matriculas)
                    .ThenInclude(m => m.Aluno)
                .FirstOrDefaultAsync(t => t.Nome == nome);
        }

        public async Task<Turma?> ObterPorIdAsync(int id)
        {
            return await _dbContext.Turmas
                .Include(t => t.Matriculas)
                    .ThenInclude(m => m.Aluno)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Turma>> ObterTodasAsync()
        {
            return await _dbContext.Turmas.ToListAsync();
        }

        public async Task<Turma> CadastrarAsync(Turma turma)
        {
            if (turma == null) throw new ArgumentNullException(nameof(turma));
            await _dbContext.Turmas.AddAsync(turma);
            await _dbContext.SaveChangesAsync();
            return turma;
        }

        public async Task AtualizarAsync(Turma turma)
        {
            if (turma == null) throw new ArgumentNullException(nameof(turma));
            _dbContext.Turmas.Update(turma);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var turma = await _dbContext.Turmas.FindAsync(id);
            if (turma == null) return;
            _dbContext.Turmas.Remove(turma);
            await _dbContext.SaveChangesAsync();
        }
    }
}
