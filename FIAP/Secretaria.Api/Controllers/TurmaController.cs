using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Commands;
using Secretaria.Application.Interfaces.Turma.Queries;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class TurmaController : ControllerBase
    {
        private readonly IObterTurmaComListaAlunosUseCase _listaAlunosUseCase;
        private readonly ICriarTurmaUseCase _criarTurmaUseCase;
        private readonly IObterTodasTurmasUseCase _obterTodasTurmasUseCase;
        private readonly IAtualizarTurmaUseCase _atualizarTurmaUseCase;
        private readonly IRemoverTurmaUseCase _removerTurmaUseCase;
        private readonly IValidator<TurmaRequestDto> _turmaValidator;

        public TurmaController(
            IObterTurmaComListaAlunosUseCase listaAlunosUseCase, 
            ICriarTurmaUseCase criarTurmaUseCase,
            IObterTodasTurmasUseCase obterTodasTurmasUseCase,
            IAtualizarTurmaUseCase atualizarTurmaUseCase,
            IRemoverTurmaUseCase removerTurmaUseCase,
            IValidator<TurmaRequestDto> turmaValidator)
        {
            _listaAlunosUseCase = listaAlunosUseCase ?? throw new ArgumentNullException(nameof(listaAlunosUseCase));
            _criarTurmaUseCase = criarTurmaUseCase ?? throw new ArgumentNullException(nameof(criarTurmaUseCase));
            _obterTodasTurmasUseCase = obterTodasTurmasUseCase ?? throw new ArgumentNullException(nameof(obterTodasTurmasUseCase));
            _atualizarTurmaUseCase = atualizarTurmaUseCase ?? throw new ArgumentNullException(nameof(atualizarTurmaUseCase));
            _removerTurmaUseCase = removerTurmaUseCase ?? throw new ArgumentNullException(nameof(removerTurmaUseCase));
            _turmaValidator = turmaValidator ?? throw new ArgumentNullException(nameof(turmaValidator));
        }

        [HttpPost("criar-turma")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarTurma([FromBody] TurmaRequestDto request)
        {
            var validationResult = await _turmaValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var erros = validationResult.Errors
                    .Select(e => new { Campo = e.PropertyName, Erro = e.ErrorMessage });

                return StatusCode(StatusCodes.Status400BadRequest, new { erros });
            }

            try
            {
                await _criarTurmaUseCase.ExecuteAsync(request);

                return StatusCode(StatusCodes.Status201Created, new { mensagem = "Turma criada com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = ex.Message });
            }
        }

        [HttpGet("/turma/todas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodasTurmas()
        {
            try
            {
                var turmas = await _obterTodasTurmasUseCase.ExecuteAsync(1, 10);

                return StatusCode(StatusCodes.Status200OK, turmas);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = ex.Message });
            }
        }

        [HttpGet("/turma/{id}/alunos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTurmaComListaAlunos([FromRoute] int id)
        {
            try
            {
                var turmaDto = await _listaAlunosUseCase.ExecuteAsync(id, 1, 10);

                return StatusCode(StatusCodes.Status200OK, turmaDto);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = ex.Message });
            }
        }

        [HttpPut("/turma/atualizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarCadastro([FromBody] TurmaRequestDto request)
        {
            var validationResult = await _turmaValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var erros = validationResult.Errors
                    .Select(e => new { Campo = e.PropertyName, Erro = e.ErrorMessage });

                return StatusCode(StatusCodes.Status400BadRequest, new { erros });
            }

            try
            {
                var alunoAtualizado = await _atualizarTurmaUseCase.ExecuteAsync(request);

                return StatusCode(StatusCodes.Status200OK, alunoAtualizado);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { erro = ex.Message });
            }
        }

        [HttpDelete("/turma/id/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoverTurma([FromRoute] int id)
        {
            try
            {
                await _removerTurmaUseCase.ExecuteAsync(id);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { erro = ex.Message });
            }
        }
    }
}
