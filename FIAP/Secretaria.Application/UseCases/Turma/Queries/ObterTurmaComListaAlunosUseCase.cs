using Secretaria.Application.Dtos.Shared;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Queries;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Turma.Queries
{
    public class ObterTurmaComListaAlunosUseCase : IObterTurmaComListaAlunosUseCase
    {
        public readonly ITurmaRepository _turmaRepository;

        public ObterTurmaComListaAlunosUseCase(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        public async Task<ResultadoPaginadoDto<TurmaDto>> ExecuteAsync(int turmaId, int page = 1, int pageSize = 10)
        {
            var turma = await _turmaRepository.ObterPorIdAsync(turmaId);

            if (turma == null)
                throw new InvalidOperationException($"Turma com ID '{turmaId}' não encontrada.");

            var alunosAtivos = turma.Matriculas
                .Where(m => m.Ativa)
                .OrderBy(m => m.Aluno.Nome)
                .ToList();

            var totalAlunos = alunosAtivos.Count;
            var totalPaginas = (int)Math.Ceiling(totalAlunos / (double)pageSize);

            var alunosPaginados = alunosAtivos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new AlunoTurmaDto
                {
                    Id = m.Aluno.Id,
                    Nome = m.Aluno.Nome
                })
                .ToList();

            var turmaDto = new TurmaDto
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                Alunos = alunosPaginados
            };

            return new ResultadoPaginadoDto<TurmaDto>
            {
                Itens = new List<TurmaDto> { turmaDto },
                Pagina = page,
                TotalPaginas = totalPaginas,
                TotalItens = totalAlunos
            };
        }
    }
}
