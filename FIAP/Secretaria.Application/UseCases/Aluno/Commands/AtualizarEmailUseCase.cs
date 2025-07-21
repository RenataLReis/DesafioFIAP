using Secretaria.Application.Dtos.Aluno;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Commands
{
    public class AtualizarEmailUseCase
    {
        private readonly IAlunoRepository _alunoRepository;
        public AtualizarEmailUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<AlunoDto> ExecuteAsync(int alunoId, string novoEmail)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(alunoId);

            if (aluno == null)
                throw new InvalidOperationException($"Aluno com ID '{alunoId}' não encontrado.");

            aluno.AtualizarEmail(novoEmail);

            await _alunoRepository.AtualizarAsync(aluno);

            return new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                DataNascimento = aluno.DataNascimento,
                Email = aluno.Email,
                CPF = aluno.CPF
            };
        }   
    }
}
