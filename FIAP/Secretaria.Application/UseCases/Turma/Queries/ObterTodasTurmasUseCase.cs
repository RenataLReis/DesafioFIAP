using Secretaria.Application.Dtos.Shared;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Queries;
using Secretaria.Domain.Interfaces;


namespace Secretaria.Application.UseCases.Turma.Queries
{
    public class ObterTodasTurmasUseCase : IObterTodasTurmasUseCase
    {
        private readonly ITurmaRepository _turmaRepository;

        public ObterTodasTurmasUseCase(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        public async Task<ResultadoPaginadoDto<TurmaDto>> ExecuteAsync(int page = 1, int pageSize = 10)
        {           
            var turmas = await _turmaRepository.ObterTodasAsync();

            if (turmas == null || !turmas.Any())
                throw new InvalidOperationException("Nenhuma turma encontrada.");

            var turmasOrdenadas = turmas.OrderBy(m => m.Nome).ToList();

            var totalTurmas = turmasOrdenadas.Count;
            var totalPaginas = (int)Math.Ceiling(totalTurmas / (double)pageSize);

            var turmasPaginadas = turmasOrdenadas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var alunosDto = turmasPaginadas.Select(turma => new TurmaDto
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao ?? string.Empty
            }).ToList();

            return new ResultadoPaginadoDto<TurmaDto>
            {
                Itens = alunosDto,
                Pagina = page,
                TotalPaginas = totalPaginas,
                TotalItens = totalTurmas
            };

        }
    }
}
