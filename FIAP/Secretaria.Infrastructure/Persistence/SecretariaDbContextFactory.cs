using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Secretaria.Infrastructure.Persistence
{
    public class SecretariaDbContextFactory : IDesignTimeDbContextFactory<SecretariaDbContext>
    {
        public SecretariaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SecretariaDbContext>();

            var connectionString = "Server=localhost;Database=FIAP;User Id=sa;Password=Lab-1005;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new SecretariaDbContext(optionsBuilder.Options);
        }
    }
}
