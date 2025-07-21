using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.UseCases.Aluno.Commands;
using Secretaria.Application.UseCases.Aluno.Queries;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class AlunoController : ControllerBase
    {
        IValidator<CadastrarAlunoRequestDto> _cadastrarAlunoValidator;
        IValidator<string> _emailValidator;
        IValidator<string> _senhaValidator;
        public readonly CadastrarAlunoUseCase _cadastrarAlunoUseCase;
        public readonly ObterAlunoPorNomeUseCase _obterAlunoPorNomeUseCase;
        public readonly ObterAlunoPorCpfUseCase _obterAlunoPorCpfUseCase;
        public readonly AtualizarEmailUseCase _atualizarEmailUseCase;
        public readonly AtualizarSenhaUseCase _atualizarSenhaUseCase;

        public AlunoController(
            IValidator<CadastrarAlunoRequestDto> cadastrarAlunoValidator,
            IValidator<string> emailValidator,
            IValidator<string> senhaValidator,
            CadastrarAlunoUseCase cadastrarAlunoUseCase, 
            ObterAlunoPorNomeUseCase obterAlunoPorNomeUseCase, 
            ObterAlunoPorCpfUseCase obterAlunoPorCpfUseCase,
            AtualizarEmailUseCase atualizarEmailUseCase,
            AtualizarSenhaUseCase atualizarSenhaUseCase)
        {
            _cadastrarAlunoValidator = cadastrarAlunoValidator ?? throw new ArgumentNullException(nameof(cadastrarAlunoValidator));
            _emailValidator = emailValidator ?? throw new ArgumentNullException(nameof(emailValidator));
            _senhaValidator = senhaValidator ?? throw new ArgumentNullException(nameof(senhaValidator));
            _cadastrarAlunoUseCase = cadastrarAlunoUseCase ?? throw new ArgumentNullException(nameof(cadastrarAlunoUseCase));
            _obterAlunoPorNomeUseCase = obterAlunoPorNomeUseCase ?? throw new ArgumentNullException(nameof(obterAlunoPorNomeUseCase));
            _obterAlunoPorCpfUseCase = obterAlunoPorCpfUseCase ?? throw new ArgumentNullException(nameof(obterAlunoPorCpfUseCase));
            _atualizarEmailUseCase = atualizarEmailUseCase ?? throw new ArgumentNullException(nameof(atualizarEmailUseCase));
            _atualizarSenhaUseCase = atualizarSenhaUseCase ?? throw new ArgumentNullException(nameof(atualizarSenhaUseCase));
        }

        [HttpPost("/cadastrar-aluno")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarAluno([FromBody] CadastrarAlunoRequestDto request)
        {
            await _cadastrarAlunoValidator.ValidateAsync(request);

            try
            {
                await _cadastrarAlunoUseCase.ExecuteAsync(request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = ex.Message });
            }
        }

        [HttpGet("/aluno/nome/{nome}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterAlunoPorNome([FromRoute] string nome)
        {
            if (string.IsNullOrEmpty(nome))
                    return StatusCode(StatusCodes.Status400BadRequest, new { erro = "Nome do aluno não informado." });

            try
            {
                var aluno = await _obterAlunoPorNomeUseCase.ExecuteAsync(nome);

                if (aluno == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { erro = $"Aluno com nome '{nome}' não encontrado." });

                return StatusCode(StatusCodes.Status200OK, aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { erro = ex.Message });
            }
        }

        [HttpGet("/aluno/cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterAlunoPorCpf([FromRoute] string cpf)
        {   
            if (string.IsNullOrEmpty(cpf))
                return StatusCode(StatusCodes.Status400BadRequest, new { erro = "CPF do aluno não informado." });

            try
            {               
                var aluno = await _obterAlunoPorCpfUseCase.ExecuteAsync(cpf);

                if (aluno == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { erro = $"Aluno com CPF '{cpf}' não encontrado." });

                return StatusCode(StatusCodes.Status200OK, aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { erro = ex.Message });
            }
        }

        [HttpPatch("/aluno/{id}/atualizar-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarEmail([FromRoute] int id, [FromBody] string novoEmail)
        {       
            await _emailValidator.ValidateAsync(novoEmail);

            try
            {
                var alunoAtualizado = await _atualizarEmailUseCase.ExecuteAsync(id, novoEmail);

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

        [HttpPatch("/aluno/{id}/atualizar-senha")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarSenha([FromRoute] int id, [FromBody] string novaSenha)
        {
            await _senhaValidator.ValidateAsync(novaSenha);

            try
            {                           
                await _atualizarSenhaUseCase.ExecuteAsync(id, novaSenha);

                return StatusCode(StatusCodes.Status204NoContent);
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
