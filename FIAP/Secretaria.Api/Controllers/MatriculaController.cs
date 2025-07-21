using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.Dtos.Matricula;
using Secretaria.Application.Interfaces.Matricula.Commands;
using Secretaria.Application.UseCases.Matricula.Commands;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatricularAlunoUseCase _matricularAlunoUseCase;
        private readonly ITrancarMatriculaUseCase _trancarMatriculaUseCase;

        public MatriculaController(MatricularAlunoUseCase matricularAlunoUseCase, TrancarMatriculaUseCase trancarMatriculaUseCase)
        {
            _matricularAlunoUseCase = matricularAlunoUseCase ?? throw new ArgumentNullException(nameof(matricularAlunoUseCase));
            _trancarMatriculaUseCase = trancarMatriculaUseCase ?? throw new ArgumentNullException(nameof(trancarMatriculaUseCase));
        }

        [HttpPost("/matricular-aluno")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MatricularAluno([FromBody] MatricularAlunoRequestDto requestDto)
        {
            try
            {
                var matriculaDto = await _matricularAlunoUseCase.ExecuteAsync(requestDto);

                return StatusCode(StatusCodes.Status200OK, new { matricula = matriculaDto });
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

        [HttpPatch("/trancar-matricula/{numero}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TrancarMatricula([FromRoute] int id)
        {
            try
            {
                await _trancarMatriculaUseCase.ExecuteAsync(id);

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
