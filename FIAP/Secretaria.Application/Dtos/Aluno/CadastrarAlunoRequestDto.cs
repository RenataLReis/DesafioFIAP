namespace Secretaria.Application.Dtos.Aluno
{
    public class CadastrarAlunoRequestDto
    {
        public required string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
        public required string Senha { get; set; }
    }
}
