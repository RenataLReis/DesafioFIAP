using FluentAssertions;
using Moq;
using Secretaria.Application.UseCases.Turma.Queries;
using Secretaria.Domain.Interfaces;
using Secretaria.Tests.UnitTests.Builders;
using Xunit;

namespace Secretaria.Tests.UnitTests.UseCases.Turma.Queries
{
    public class ObterTurmaComListaAlunosUseCaseTests
    {
        private readonly Mock<ITurmaRepository> _turmaRepositoryMock;
        private readonly ObterTurmaComListaAlunosUseCase _useCase;

        public ObterTurmaComListaAlunosUseCaseTests()
        {
            _turmaRepositoryMock = new Mock<ITurmaRepository>();
            _useCase = new ObterTurmaComListaAlunosUseCase(_turmaRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarTurmaComAlunosPaginados_QuandoDadosForemValidos()
        {
            var alunos = Enumerable.Range(1, 25).Select(i =>
                new AlunoBuilder().ComNome($"Aluno {i:00}").ComId(i).Build()).ToList();

            var turma = new TurmaBuilder().ComId(1).ComNome("Turma A").ComDescricao("Teste").Build();

            for (int i = 0; i < alunos.Count; i++)
            {
                var matricula = new MatriculaBuilder()
                    .ComAluno(alunos[i])
                    .ComTurma(turma)
                    .ComNumero($"2025{i:0000}")
                    .Build();

                turma.Matriculas.Add(matricula);
            }

            _turmaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(turma);

            var resultado = await _useCase.ExecuteAsync(1, page: 2, pageSize: 10);

            resultado.Should().NotBeNull();
            resultado.Itens.Should().HaveCount(1);
            var turmaDto = resultado.Itens.First();
            turmaDto.Alunos.Should().HaveCount(10);
            turmaDto.Alunos.First().Nome.Should().Be("Aluno 11");
            resultado.Pagina.Should().Be(2);
            resultado.TotalItens.Should().Be(25);
            resultado.TotalPaginas.Should().Be(3);
        }

        [Fact]
        public async Task ExecuteAsync_DeveLancarExcecao_QuandoTurmaNaoExistir()
        {
            _turmaRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Turma)null!);

            Func<Task> act = async () => await _useCase.ExecuteAsync(1);

            await act.Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage("Turma com ID '1' não encontrada.");
        }
    }
}
