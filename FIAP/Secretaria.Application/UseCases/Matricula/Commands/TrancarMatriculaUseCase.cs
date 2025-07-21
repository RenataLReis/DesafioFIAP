using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Matricula.Commands
{
    public class TrancarMatriculaUseCase
    {
        private readonly IMatriculaRepository _matriculaRepository;
        public TrancarMatriculaUseCase(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository ?? throw new ArgumentNullException(nameof(matriculaRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(id);
            if (matricula == null)
                throw new InvalidOperationException($"Matrícula com ID '{id}' não encontrada.");

            matricula.TrancarMatricula(matricula.Numero);

            await _matriculaRepository.AtualizarAsync(matricula);
        }
    }
}
