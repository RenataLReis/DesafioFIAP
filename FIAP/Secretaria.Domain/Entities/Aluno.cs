namespace Secretaria.Domain.Entities
{
    public class Aluno
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Email { get; private set; }
        public string CPF { get; private set; }
        public string SenhaHash { get; private set; }

        protected Aluno() { }

        public Aluno(string nome, DateTime dataNascimento, string email, string cpf, string senhaHash)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            CPF = cpf ?? throw new ArgumentNullException(nameof(cpf));
            SenhaHash = senhaHash ?? throw new ArgumentNullException(nameof(senhaHash));
            if (dataNascimento == default || dataNascimento > DateTime.Today)
                throw new ArgumentException("Data de nascimento inválida.", nameof(dataNascimento));
            DataNascimento = dataNascimento;
        }

        public static Aluno Criar(string nome, DateTime dataNascimento, string email, string cpf, string senhaHash)
        {
            return new Aluno(nome, dataNascimento, email, cpf, senhaHash);
        }

        public void AtualizarEmail(string email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public void AtualizarSenha(string senhaHash)
        {
            SenhaHash = senhaHash ?? throw new ArgumentNullException(nameof(senhaHash));
        }
    }
}
