using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.UseCases.Turma.Queries;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class TurmaController : ControllerBase
    {
        private readonly ObterTurmaComListaAlunosUseCase _listaAlunosUseCase;

        public TurmaController(ObterTurmaComListaAlunosUseCase listaAlunosUseCase)
        {
            _listaAlunosUseCase = listaAlunosUseCase ?? throw new ArgumentNullException(nameof(listaAlunosUseCase));
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

                return Ok(turmaDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensagem = ex.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = ex.Message });
            }
        }
    }
}
