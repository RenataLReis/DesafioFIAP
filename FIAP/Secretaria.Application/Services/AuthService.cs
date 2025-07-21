using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Secretaria.Domain.Interfaces;
using Secretaria.Domain.Entities;
using Secretaria.Application.Dtos.Administrador;


namespace Secretaria.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAdministradorRepository _administradorRepository;

        public AuthService(IConfiguration configuration, IAdministradorRepository administradorRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _administradorRepository = administradorRepository ?? throw new ArgumentNullException(nameof(administradorRepository));
        }

        public async Task<string> GerarToken(GerarTokenRequestDto request)
        {
            var admin = await _administradorRepository.ObterPorEmailAsync(request.Email);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Senha, admin?.SenhaHash))           
                throw new UnauthorizedAccessException();           

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.Email),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("role", "Admin")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task CadastrarAdminAsync(AdminRequestDto request)
        {
            var adminExistente = await _administradorRepository.ObterPorEmailAsync(request.Email);

            if (adminExistente != null)
                throw new InvalidOperationException("Já existe um administrador com esse e-mail.");

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var novoAdmin = new Administrador(request.Email, senhaHash);

            await _administradorRepository.CadastrarAsync(novoAdmin);
        }

        public async Task ExcluirAdminAsync(AdminRequestDto request)
        {
            var admin = await _administradorRepository.ObterPorEmailAsync(request.Email);

            if (admin == null)
                throw new InvalidOperationException("Administrador não encontrado.");

            await _administradorRepository.RemoverAsync(admin);
        }
    }

}
