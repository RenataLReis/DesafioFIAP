using Secretaria.Domain.Entities;

namespace Secretaria.Domain.Interfaces
{
    public interface IAdministradorRepository
    {
        Task CadastrarAsync(Administrador administrador);
        Task<Administrador?> ObterPorEmailAsync(string email);        
        Task RemoverAsync(Administrador administrador);
    }
}
