namespace Secretaria.Domain.Entities
{
    public class Matricula
    {
        public int Id { get; private set; }
        public int AlunoId { get; private set; }
        public Aluno Aluno { get; private set; } 
        public int TurmaId { get; private set; }
        public Turma Turma { get; private set; }  
        public string Numero { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAlteracao { get; private set; }
        public bool Ativa { get; private set; }

        protected Matricula() { }

        private Matricula(int alunoId, int turmaId, string numero)
        {
            AlunoId = alunoId;
            TurmaId = turmaId;
            Numero = numero ?? throw new ArgumentNullException(nameof(numero));
            DataCriacao = DateTime.UtcNow;
            Ativa = true;
        }

        public static Matricula Criar(int alunoId, int turmaId, string numero)
        {
            return new Matricula(alunoId, turmaId, numero);
        }

        public void TrancarMatricula(string numero)
        {
            if (!Ativa)
                throw new InvalidOperationException("A matrícula já está inativa.");
            Ativa = false;
            DataAlteracao = DateTime.UtcNow;
        }
    }
}
