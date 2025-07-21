using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Commands
{
    public class AtualizarSenhaUseCase
    {
        private readonly IAlunoRepository _alunoRepository;

        public AtualizarSenhaUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task ExecuteAsync(int alunoId, string novaSenhaHash)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(alunoId);

            if (aluno == null)
                throw new InvalidOperationException($"Aluno com ID '{alunoId}' não encontrado.");

            aluno.AtualizarSenha(novaSenhaHash);

            await _alunoRepository.AtualizarAsync(aluno);
        }
    }
}
