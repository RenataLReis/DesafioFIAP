using Secretaria.Application.Dtos.Matricula;
using Secretaria.Application.Interfaces.Matricula.Commands;
using Secretaria.Application.Services;
using Secretaria.Domain.Interfaces;

namespace Secretaria.Application.UseCases.Matricula.Commands
{
    public class MatricularAlunoUseCase : IMatricularAlunoUseCase
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly IMatriculaRepository _matriculaRepository; 
        private readonly IGeradorNumeroMatricula _geradorNumeroMatricula;

        public MatricularAlunoUseCase(
            IAlunoRepository alunoRepository, 
            ITurmaRepository turmaRepository, 
            IMatriculaRepository matriculaRepository,
            IGeradorNumeroMatricula geradorNumeroMatricula)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
            _matriculaRepository = matriculaRepository ?? throw new ArgumentNullException(nameof(matriculaRepository));
            _geradorNumeroMatricula = geradorNumeroMatricula ?? throw new ArgumentNullException(nameof(geradorNumeroMatricula));
        }

        public async Task<MatriculaDto> ExecuteAsync(MatricularAlunoRequestDto requestDto)
        {           
            var aluno = await _alunoRepository.ObterPorIdAsync(requestDto.AlunoId);

            if (aluno == null)
                throw new InvalidOperationException($"Aluno com ID '{requestDto.AlunoId}' não encontrado.");

            var turma = await _turmaRepository.ObterPorIdAsync(requestDto.TurmaId);

            if (turma == null)
                throw new InvalidOperationException($"Turma com ID '{requestDto.TurmaId}' não encontrada.");

            var ano = DateTime.Now.Year.ToString().Substring(2,2);

            var ultimaMatriculaAno = await _matriculaRepository.ObterUltimoNumeroAsync(ano) ?? "0";

            var numeroMatricula = _geradorNumeroMatricula.Gerar(ultimaMatriculaAno, ano);

            var matricula = Domain.Entities.Matricula.Criar(requestDto.AlunoId, requestDto.TurmaId, numeroMatricula);

            await _matriculaRepository.CriarAsync(matricula);

            return new MatriculaDto
            {
                Id = matricula.Id,
                AlunoId = matricula.AlunoId,
                AlunoNome = aluno.Nome,
                TurmaId = matricula.TurmaId,
                NomeTurma = turma.Nome,
                Numero = matricula.Numero,
                Ativa = matricula.Ativa
            };
        }
    }
}
