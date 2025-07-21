using FluentValidation;
using Secretaria.Application.Interfaces.Aluno.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Commands
{
    public class AtualizarSenhaUseCase : IAtualizarSenhaUseCase
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IValidator<string> _senhaValidator;

        public AtualizarSenhaUseCase(IAlunoRepository alunoRepository, IValidator<string> senhaValidator)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
            _senhaValidator = senhaValidator ?? throw new ArgumentNullException(nameof(senhaValidator));
        }

        public async Task ExecuteAsync(int alunoId, string novaSenha)
        {
            await _senhaValidator.ValidateAsync(novaSenha);
            
            var aluno = await _alunoRepository.ObterPorIdAsync(alunoId);

            if (aluno == null)
                throw new InvalidOperationException($"Aluno com ID '{alunoId}' não encontrado.");

            var novaSenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);

            aluno.AtualizarSenha(novaSenhaHash);

            await _alunoRepository.AtualizarAsync(aluno);
        }
    }
}
