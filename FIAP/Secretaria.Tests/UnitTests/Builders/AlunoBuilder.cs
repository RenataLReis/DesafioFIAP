using Secretaria.Domain.Entities;

namespace Secretaria.Tests.UnitTests.Builders
{
    public class AlunoBuilder
    {
        private int _id = 1;
        private string _nome = "Aluno Teste";
        private DateTime _dataNascimento = new DateTime(2000, 1, 1);
        private string _email = "aluno@teste.com";
        private string _cpf = "12345678900";
        private string _senhaHash = "hashedpassword";

        public AlunoBuilder ComId(int id)
        {
            _id = id;
            return this;
        }

        public AlunoBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public AlunoBuilder ComDataNascimento(DateTime dataNascimento)
        {
            _dataNascimento = dataNascimento;
            return this;
        }

        public AlunoBuilder ComEmail(string email)
        {
            _email = email;
            return this;
        }

        public AlunoBuilder ComCpf(string cpf)
        {
            _cpf = cpf;
            return this;
        }

        public AlunoBuilder ComSenhaHash(string senhaHash)
        {
            _senhaHash = senhaHash;
            return this;
        }

        public Aluno Build()
        {
            var aluno = Aluno.Criar(_nome, _dataNascimento, _email, _cpf, _senhaHash);

            typeof(Aluno).GetProperty("Id")!.SetValue(aluno, _id);

            return aluno;
        }
    }
}
