using FluentAssertions;
using Moq;
using Secretaria.Application.Dtos.Matricula;
using Secretaria.Application.Services;
using Secretaria.Application.UseCases.Matricula.Commands;
using Secretaria.Domain.Interfaces;
using Secretaria.Tests.UnitTests.Builders;
using Xunit;

namespace Secretaria.Tests.UnitTests.UseCases.Matricula.Command
{
    public class MatricularAlunoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveMatricularAluno_QuandoDadosForemValidos()
        {
            var alunoId = 1;
            var turmaId = 2;
            var ano = "25";
            var ultimaMatriculaAno = "15";
            var numeroGerado = "250016";

            var aluno = new AlunoBuilder()
                .ComId(alunoId)
                .ComNome("Maria")
                .Build();

            var turma = new TurmaBuilder()
                .ComId(turmaId)
                .ComNome("Turma A")
                .ComDescricao("Teste")
                .Build();

            Domain.Entities.Matricula? matriculaCriada = null;

            var alunoRepository = new Mock<IAlunoRepository>();
            alunoRepository.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync(aluno);

            var turmaRepository = new Mock<ITurmaRepository>();
            turmaRepository.Setup(r => r.ObterPorIdAsync(turmaId)).ReturnsAsync(turma);

            var matriculaRepository = new Mock<IMatriculaRepository>();
            matriculaRepository.Setup(r => r.ObterUltimoNumeroAsync(ano)).ReturnsAsync(ultimaMatriculaAno);
            matriculaRepository.Setup(r => r.CriarAsync(It.IsAny<Domain.Entities.Matricula>()))
                .Callback<Domain.Entities.Matricula>(m =>
                {
                    matriculaCriada = m;
                    typeof(Domain.Entities.Matricula).GetProperty(nameof(m.Aluno))!.SetValue(m, aluno);
                    typeof(Domain.Entities.Matricula).GetProperty(nameof(m.Turma))!.SetValue(m, turma);
                })
                .Returns(Task.CompletedTask);

            var geradorNumero = new Mock<IGeradorNumeroMatricula>();
            geradorNumero.Setup(g => g.Gerar(ultimaMatriculaAno, ano)).Returns(numeroGerado);

            var useCase = new MatricularAlunoUseCase(
                alunoRepository.Object,
                turmaRepository.Object,
                matriculaRepository.Object,
                geradorNumero.Object
            );

            var request = new MatricularAlunoRequestDto
            {
                AlunoId = alunoId,
                TurmaId = turmaId
            };

            var result = await useCase.ExecuteAsync(request);

            result.Should().NotBeNull();
            result.AlunoId.Should().Be(alunoId);
            result.TurmaId.Should().Be(turmaId);
            result.Numero.Should().Be(numeroGerado);
            result.Ativa.Should().BeTrue();
            result.AlunoNome.Should().Be(aluno.Nome);
            result.NomeTurma.Should().Be(turma.Nome);

            matriculaCriada.Should().NotBeNull();
            matriculaCriada!.AlunoId.Should().Be(alunoId);
            matriculaCriada.TurmaId.Should().Be(turmaId);
            matriculaCriada.Numero.Should().Be(numeroGerado);
        }




        [Fact]
        public async Task ExecuteAsync_DeveLancarExcecao_QuandoAlunoNaoEncontrado()
        {
            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();
            var geradorNumeroMatriculaMock = new Mock<IGeradorNumeroMatricula>();

            alunoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Aluno)null!); 

            var useCase = new MatricularAlunoUseCase(
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object,
                matriculaRepositoryMock.Object,
                geradorNumeroMatriculaMock.Object
            );

            var request = new MatricularAlunoRequestDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_DeveLancarExcecao_QuandoTurmaNaoEncontrada()
        {
            var alunoId = 1;
            var turmaId = 99;

            var aluno = new AlunoBuilder()
                .ComId(alunoId)
                .ComNome("Maria")
                .Build();

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();
            var geradorNumeroMatriculaMock = new Mock<IGeradorNumeroMatricula>();

            alunoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(alunoId))
                .ReturnsAsync(aluno);

            turmaRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(turmaId))
                .ReturnsAsync((Domain.Entities.Turma?)null);

            var useCase = new MatricularAlunoUseCase(
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object,
                matriculaRepositoryMock.Object,
                geradorNumeroMatriculaMock.Object
            );

            var request = new MatricularAlunoRequestDto
            {
                AlunoId = alunoId,
                TurmaId = turmaId
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(request));
            ex.Message.Should().Be($"Turma com ID '{turmaId}' não encontrada.");
        }


        [Fact]
        public async Task ExecuteAsync_DeveLancarExcecao_QuandoGeradorFalhar()
        {
            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var turmaRepositoryMock = new Mock<ITurmaRepository>();
            var matriculaRepositoryMock = new Mock<IMatriculaRepository>();
            var geradorNumeroMatriculaMock = new Mock<IGeradorNumeroMatricula>();

            var aluno = new Domain.Entities.Aluno("Maria", DateTime.Parse("2000-01-01"), "maria@teste.com", "12345678900", "senha");
            var turma = new Domain.Entities.Turma("Turma A", "Descrição");

            alunoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(aluno);

            turmaRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(turma);

            matriculaRepositoryMock
                .Setup(repo => repo.ObterUltimoNumeroAsync(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            geradorNumeroMatriculaMock
                .Setup(g => g.Gerar(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>(); 

            var useCase = new MatricularAlunoUseCase(
                alunoRepositoryMock.Object,
                turmaRepositoryMock.Object,
                matriculaRepositoryMock.Object,
                geradorNumeroMatriculaMock.Object
            );

            var request = new MatricularAlunoRequestDto
            {
                AlunoId = 1,
                TurmaId = 1
            };

            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(request));
        }
    }
}
