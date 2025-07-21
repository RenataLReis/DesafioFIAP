using Secretaria.Application.Dtos.Administrador;

namespace Secretaria.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> GerarToken(GerarTokenRequestDto admin);
        Task CadastrarAdminAsync(AdminRequestDto request);
        Task ExcluirAdminAsync(AdminRequestDto request);
    }
}
