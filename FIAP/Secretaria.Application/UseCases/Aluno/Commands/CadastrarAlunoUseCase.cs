using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Interfaces.Aluno.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Commands
{
    public class CadastrarAlunoUseCase : ICadastrarAlunoUseCase
    {
        private readonly IAlunoRepository _alunoRepository;
        public CadastrarAlunoUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task ExecuteAsync(AlunoRequestDto request)
        {

            if (await _alunoRepository.ObterPorCpfAsync(request.CPF) != null)
                throw new InvalidOperationException($"O aluno com CPF '{request.CPF}' já está cadastrado.");

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var aluno = Domain.Entities.Aluno.Criar(
                request.Nome,
                request.DataNascimento,
                request.Email,
                request.CPF,
                senhaHash);

            await _alunoRepository.CadastrarAsync(aluno);
        }
    }
}
