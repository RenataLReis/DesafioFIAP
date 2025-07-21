using Secretaria.Domain.Entities;

namespace Secretaria.Tests.UnitTests.Builders
{
    public class TurmaBuilder
    {
        private int _id = 1;
        private string _nome = "Turma A";
        private string? _descricao = "Turma de testes";
        private List<Matricula> _matriculas = new();

        public TurmaBuilder ComId(int id)
        {
            _id = id;
            return this;
        }

        public TurmaBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public TurmaBuilder ComDescricao(string? descricao)
        {
            _descricao = descricao;
            return this;
        }

        public TurmaBuilder ComMatriculas(params Matricula[] matriculas)
        {
            _matriculas.AddRange(matriculas);
            return this;
        }

        public Turma Build()
        {
            var turma = Turma.Criar(_nome, _descricao);

            typeof(Turma).GetProperty("Id")!.SetValue(turma, _id);

            typeof(Turma).GetProperty("Matriculas")!
                .SetValue(turma, _matriculas);

            return turma;
        }
    }
}
