using Secretaria.Domain.Entities;

namespace Secretaria.Tests.UnitTests.Builders
{
    public class MatriculaBuilder
    {
        private int _id = 1;
        private int _alunoId = 1;
        private Aluno _aluno = new AlunoBuilder().Build();
        private int _turmaId = 1;
        private Turma _turma = new TurmaBuilder().Build();
        private string _numero = "0001/24";
        private DateTime _dataCriacao = DateTime.UtcNow;
        private DateTime? _dataAlteracao = null;
        private bool _ativa = true;

        public MatriculaBuilder ComId(int id)
        {
            _id = id;
            return this;
        }

        public MatriculaBuilder ComAluno(Aluno aluno)
        {
            _aluno = aluno;
            _alunoId = aluno?.Id ?? 1;
            return this;
        }

        public MatriculaBuilder ComTurma(Turma turma)
        {
            _turma = turma;
            _turmaId = turma?.Id ?? 1;
            return this;
        }

        public MatriculaBuilder ComNumero(string numero)
        {
            _numero = numero;
            return this;
        }

        public MatriculaBuilder CriadaEm(DateTime data)
        {
            _dataCriacao = data;
            return this;
        }

        public MatriculaBuilder ComDataAlteracao(DateTime? data)
        {
            _dataAlteracao = data;
            return this;
        }

        public MatriculaBuilder AtivaOuNao(bool ativa)
        {
            _ativa = ativa;
            return this;
        }

        public Matricula Build()
        {
            var matricula = Matricula.Criar(_alunoId, _turmaId, _numero);

            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, _id);
            typeof(Matricula).GetProperty("Aluno")!.SetValue(matricula, _aluno);
            typeof(Matricula).GetProperty("Turma")!.SetValue(matricula, _turma);
            typeof(Matricula).GetProperty("DataCriacao")!.SetValue(matricula, _dataCriacao);
            typeof(Matricula).GetProperty("DataAlteracao")!.SetValue(matricula, _dataAlteracao);
            typeof(Matricula).GetProperty("Ativa")!.SetValue(matricula, _ativa);

            return matricula;
        }
    }

}
