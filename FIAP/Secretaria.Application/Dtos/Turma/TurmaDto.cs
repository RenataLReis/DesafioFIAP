namespace Secretaria.Application.Dtos.Turma
{
    public class TurmaDto
    {
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public int Id { get; set; }
        public List<AlunoTurmaDto>? Alunos { get; set; } = new List<AlunoTurmaDto>();
    }
}
