using Moq;
using Xunit;
using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.UseCases.Aluno.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Tests.UnitTests.UseCases.Aluno.Command
{
    public class CadastrarAlunoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveCadastrarAluno_QuandoCpfNaoExiste()
        {
            var alunoRepositoryMock = new Mock<IAlunoRepository>();

            alunoRepositoryMock.Setup(repo => repo.ObterPorCpfAsync(It.IsAny<string>()))
                .ReturnsAsync((Domain.Entities.Aluno)(null!));

            var useCase = new CadastrarAlunoUseCase(alunoRepositoryMock.Object);

            var request = new AlunoRequestDto
            {
                Nome = "Maria Teste",
                CPF = "12345678900",
                Email = "maria@teste.com",
                Senha = "SenhaForte123!",
                DataNascimento = new DateTime(2000, 1, 1)
            };

            await useCase.ExecuteAsync(request);
           
            alunoRepositoryMock.Verify(repo => repo.CadastrarAsync(It.Is<Domain.Entities.Aluno>(a =>
                a.Nome == request.Nome &&
                a.Email == request.Email &&
                a.CPF == request.CPF
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_DeveLancarExcecao_QuandoCpfJaExistente()
        {
            var alunoExistente = Domain.Entities.Aluno.Criar("João", new DateTime(2001, 1, 1), "joao@teste.com", "12345678900", "hash");

            var alunoRepositoryMock = new Mock<IAlunoRepository>();

            alunoRepositoryMock.Setup(repo => repo.ObterPorCpfAsync("12345678900"))
                .ReturnsAsync(alunoExistente);

            var useCase = new CadastrarAlunoUseCase(alunoRepositoryMock.Object);

            var request = new AlunoRequestDto
            {
                Nome = "Outro Nome",
                CPF = "12345678900",
                Email = "outro@teste.com",
                Senha = "OutraSenha123!",
                DataNascimento = new DateTime(2002, 1, 1)
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(request));

            Assert.Equal("O aluno com CPF '12345678900' já está cadastrado.", exception.Message);

            alunoRepositoryMock.Verify(repo => repo.CadastrarAsync(It.IsAny<Domain.Entities.Aluno>()), Times.Never);
        }
    }

}
