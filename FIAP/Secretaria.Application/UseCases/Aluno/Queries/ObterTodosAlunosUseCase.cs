using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Dtos.Shared;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Aluno.Queries;
using Secretaria.Domain.Entities;
using Secretaria.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.Application.UseCases.Aluno.Queries
{
    public class ObterTodosAlunosUseCase : IObterTodosAlunosUseCase
    {
        private readonly IAlunoRepository _alunoRepository;

        public ObterTodosAlunosUseCase(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<ResultadoPaginadoDto<AlunoDto>> ExecuteAsync(int page = 1, int pageSize = 10)
        {
            var alunos = await _alunoRepository.ObterTodosAsync();

            if (alunos == null || !alunos.Any())
                throw new InvalidOperationException("Nenhum aluno encontrado.");

            var alunosOrdenados = alunos.OrderBy(m => m.Nome).ToList();

            var totalAlunos = alunosOrdenados.Count;
            var totalPaginas = (int)Math.Ceiling(totalAlunos / (double)pageSize);

            var alunosPaginados = alunosOrdenados
                .Skip((page - 1) * pageSize)
                .Take(pageSize)               
                .ToList();

            var alunosDto = alunosPaginados.Select(aluno => new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                CPF = aluno.CPF,
                DataNascimento = aluno.DataNascimento
            }).ToList();

            return new ResultadoPaginadoDto<AlunoDto>
            {
                Itens = alunosDto,
                Pagina = page,
                TotalPaginas = totalPaginas,
                TotalItens = totalAlunos
            };
        }
    }
}
