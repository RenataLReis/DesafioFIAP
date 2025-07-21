namespace Secretaria.Domain.Entities
{
    public class Turma
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
        public ICollection<Matricula> Matriculas { get; private set; } = new List<Matricula>();

        protected Turma() { }

        public Turma(string nome, string? descricao)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Descricao = descricao;
        }

        public static Turma Criar(string nome, string? descricao)
        {
            return new Turma(nome, descricao);
        }

        public void Excluir()
        {
            // Implementar lógica de exclusão, se necessário
            // Por exemplo, definir um estado de exclusão ou remover da coleção
            // No entanto, a exclusão física pode não ser recomendada em sistemas reais
            throw new NotImplementedException("Lógica de exclusão não implementada.");
        }
    }
}
