using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Turma.Commands;
using Secretaria.Application.Interfaces.Turma.Queries;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class TurmaController : ControllerBase
    {
        private readonly IObterTurmaComListaAlunosUseCase _listaAlunosUseCase;
        private readonly ICriarTurmaUseCase _criarTurmaUseCase;
        private readonly IValidator<TurmaRequestDto> _turmaValidator;

        public TurmaController(
            IObterTurmaComListaAlunosUseCase listaAlunosUseCase, 
            ICriarTurmaUseCase criarTurmaUseCase, 
            IValidator<TurmaRequestDto> turmaValidator)
        {
            _listaAlunosUseCase = listaAlunosUseCase ?? throw new ArgumentNullException(nameof(listaAlunosUseCase));
            _criarTurmaUseCase = criarTurmaUseCase ?? throw new ArgumentNullException(nameof(criarTurmaUseCase));
            _turmaValidator = turmaValidator ?? throw new ArgumentNullException(nameof(turmaValidator));
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

        [HttpPost("criar-turma")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarTurma([FromBody] TurmaRequestDto request)
        {
            await _turmaValidator.ValidateAsync(request);

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
    }
}
