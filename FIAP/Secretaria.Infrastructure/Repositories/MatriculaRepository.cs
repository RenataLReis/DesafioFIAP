using Microsoft.EntityFrameworkCore;
using Secretaria.Domain.Entities;
using Secretaria.Domain.Interfaces;
using Secretaria.Infrastructure.Persistence;

namespace Secretaria.Infrastructure.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        public readonly SecretariaDbContext _dbContext;

        public MatriculaRepository(SecretariaDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CriarAsync(Matricula matricula)
        {
            if (matricula == null) throw new ArgumentNullException(nameof(matricula));
            await _dbContext.Matriculas.AddAsync(matricula);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Matricula matricula)
        {
            if (matricula == null) throw new ArgumentNullException(nameof(matricula));
            _dbContext.Matriculas.Update(matricula);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var matricula = await _dbContext.Matriculas.FindAsync(id);
            if (matricula == null) return;
            _dbContext.Matriculas.Remove(matricula);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Matricula?> ObterPorIdAsync(int id)
        {
            return await _dbContext.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matricula>> ObterTodasAsync()
        {
            return await _dbContext.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> ObterPorAlunoIdAsync(int alunoId)
        {
            return await _dbContext.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                .Where(m => m.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> ObterPorTurmaIdAsync(int turmaId)
        {
            return await _dbContext.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                .Where(m => m.TurmaId == turmaId)
                .ToListAsync();
        }

        public async Task<string?> ObterUltimoNumeroAsync(string ano)
        {
            return await _dbContext.Matriculas
                .Where(m => m.Numero.StartsWith(ano))
                .OrderByDescending(m => m.Numero)
                .Select(m => m.Numero)
                .FirstOrDefaultAsync();
        }
    }
}
