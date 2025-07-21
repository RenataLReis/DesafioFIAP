using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Interfaces.Aluno.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Commands
{
    public class AtualizarCadastroAlunoUseCase : IAtualizarCadastroAlunoUseCase
    {
        private readonly IAlunoRepository _alunoRepository;
        public AtualizarCadastroAlunoUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<AlunoDto> ExecuteAsync(AlunoRequestDto request)
        {
            var aluno = await _alunoRepository.ObterPorCpfAsync(request.CPF);

            if (aluno == null)
                throw new InvalidOperationException($"Aluno com CPF '{request.CPF}' não encontrado.");

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var alunoAtualizado = Domain.Entities.Aluno.Criar(
                aluno.Nome,
                aluno.DataNascimento,
                aluno.Email,
                aluno.CPF,
                senhaHash);

            await _alunoRepository.AtualizarAsync(alunoAtualizado);

            return new AlunoDto
            {
                Id = alunoAtualizado.Id,
                Nome = alunoAtualizado.Nome,
                DataNascimento = alunoAtualizado.DataNascimento,
                Email = alunoAtualizado.Email,
                CPF = alunoAtualizado.CPF
            };
        }   
    }
}
