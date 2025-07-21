using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Queries;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Turma.Queries
{
    public class ObterTurmaComListaAlunosUseCase : IObterTurmaComListaAlunosUseCase
    {
        public readonly ITurmaRepository _turmaRepository;
        public readonly IAlunoRepository _alunoRepository;

        public ObterTurmaComListaAlunosUseCase(ITurmaRepository turmaRepository, IAlunoRepository alunoRepository)
        {
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<TurmaDto> ExecuteAsync(int turmaId, int page = 1, int pageSize = 10)
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

            return new TurmaDto
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                Alunos = alunosPaginados,
                TotalAlunos = totalAlunos,
                TotalPaginas = totalPaginas,
                PaginaAtual = page
            };
        }
    }
}
