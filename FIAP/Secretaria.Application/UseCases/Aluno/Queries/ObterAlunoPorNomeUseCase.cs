using Secretaria.Application.Interfaces.Aluno.Queries;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Aluno.Queries
{
    public class ObterAlunoPorNomeUseCase : IObterAlunoPorNomeUseCase
    {
        public readonly IAlunoRepository _alunoRepository;

        public ObterAlunoPorNomeUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<Domain.Entities.Aluno?> ExecuteAsync(string nome)
        {           
            return await _alunoRepository.ObterPorNomeAsync(nome);
        }
    }
}
