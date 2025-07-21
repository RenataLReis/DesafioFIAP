using Secretaria.Application.Interfaces.Aluno.Commands;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Commands
{
    public class RemoverAlunoUseCase : IRemoverAlunoUseCase
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMatriculaRepository _matriculaRepository;

        public RemoverAlunoUseCase(IAlunoRepository alunoRepository, IMatriculaRepository matriculaRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
            _matriculaRepository = matriculaRepository ?? throw new ArgumentNullException(nameof(matriculaRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(id);

            if (aluno == null)
                throw new InvalidOperationException($"Aluno com ID '{id}' não encontrado.");

            await _alunoRepository.RemoverAsync(id);

            var matriculas = await _matriculaRepository.ObterPorAlunoIdAsync(id);

            if (matriculas != null && matriculas.Any())
            {
                foreach (var matricula in matriculas)
                {
                    await _matriculaRepository.RemoverAsync(matricula.Id);
                }
            }
        }
    }
}
