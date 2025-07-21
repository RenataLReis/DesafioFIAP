using System;
namespace Secretaria.Domain.Entities
{
    public class Administrador
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;

        public Administrador(string email, string senhaHash) 
        {
            Email = email;
            SenhaHash = senhaHash;
        }
    }
}
