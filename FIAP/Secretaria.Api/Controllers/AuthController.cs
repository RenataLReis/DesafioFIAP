using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.Dtos.Administrador;
using Secretaria.Application.Services;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("auth/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ;
        }

        [AllowAnonymous]
        [HttpPost("gerar-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GerarToken(GerarTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = "Email e senha são obrigatórios." });

            try
            {
                var token = await _authService.GerarToken(request);
                return StatusCode(StatusCodes.Status200OK, new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { erro = "Credenciais inválidas." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { erro = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("cadastrar-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cadastrar(AdminRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = "Email e senha são obrigatórios." });

            try
            {
                await _authService.CadastrarAdminAsync(request);

                return StatusCode(StatusCodes.Status201Created, new { mensagem = "Administrador cadastrado com sucesso." });
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

        [AllowAnonymous]
        [HttpDelete("excluir-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Excluir([FromBody] AdminRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = "Email é obrigatório." });

            try
            {
                await _authService.ExcluirAdminAsync(request);

                return StatusCode(StatusCodes.Status204NoContent, new { mensagem = "Administrador excluído com sucesso." });
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

    }

}
