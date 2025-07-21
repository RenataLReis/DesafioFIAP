namespace Secretaria.Application.Dtos.Administrador
{
    public class GerarTokenRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
