using Secretaria.Domain.Entities;
using System.Reflection;

namespace Secretaria.Tests.UnitTests.Helpers
{
    public static class TestEntityFactory
    {
        public static Matricula CriarMatriculaParaTeste(Aluno aluno, int turmaId, string numero, bool ativa = true)
        {
            var matricula = (Matricula)Activator.CreateInstance(typeof(Matricula), true)!;

            SetPrivateProperty(matricula, nameof(matricula.Aluno), aluno);
            SetPrivateProperty(matricula, nameof(matricula.AlunoId), aluno.Id);
            SetPrivateProperty(matricula, nameof(matricula.TurmaId), turmaId);
            SetPrivateProperty(matricula, nameof(matricula.Numero), numero);
            SetPrivateProperty(matricula, nameof(matricula.Ativa), ativa);

            return matricula;
        }

        private static void SetPrivateProperty<T>(object target, string propertyName, T value)
        {
            var property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            property?.SetValue(target, value);
        }
    }
}
