using Microsoft.EntityFrameworkCore;
using Secretaria.Domain.Interfaces;
using Secretaria.Infrastructure.Persistence;

namespace Secretaria.Infrastructure.Repositories
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly SecretariaDbContext _dbContext;
        public AdministradorRepository(SecretariaDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CadastrarAsync(Domain.Entities.Administrador administrador)
        {
            await _dbContext.Administradores.AddAsync(administrador);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Domain.Entities.Administrador?> ObterPorEmailAsync(string email)
        {
            return await _dbContext.Administradores
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task RemoverAsync(Domain.Entities.Administrador administrador)
        {
            _dbContext.Administradores.Remove(administrador);
            await _dbContext.SaveChangesAsync();
        }
    }
}
