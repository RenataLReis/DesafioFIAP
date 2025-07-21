using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Queries
{
    public class ObterAlunoPorCpfUseCase
    {
        public readonly IAlunoRepository _alunoRepository;

        public ObterAlunoPorCpfUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<Domain.Entities.Aluno?> ExecuteAsync(string cpf)
        {
            return await _alunoRepository.ObterPorCpfAsync(cpf);
        }
    }
}
