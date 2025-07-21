using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Interfaces.Aluno.Commands;
using Secretaria.Application.Interfaces.Aluno.Queries;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class AlunoController : ControllerBase
    {
        public readonly IValidator<AlunoRequestDto> _alunoValidator;
        public readonly ICadastrarAlunoUseCase _cadastrarAlunoUseCase;
        public readonly IObterAlunoPorNomeUseCase _obterAlunoPorNomeUseCase;
        public readonly IObterAlunoPorCpfUseCase _obterAlunoPorCpfUseCase;
        public readonly IAtualizarCadastroAlunoUseCase _atualizarCadastroAlunoUseCase;
        public readonly IAtualizarSenhaUseCase _atualizarSenhaUseCase;
        public readonly IRemoverAlunoUseCase _removerAlunoUseCase;


        public AlunoController(
            IValidator<AlunoRequestDto> cadastrarAlunoValidator,
            ICadastrarAlunoUseCase cadastrarAlunoUseCase,
            IObterAlunoPorNomeUseCase obterAlunoPorNomeUseCase,
            IObterAlunoPorCpfUseCase obterAlunoPorCpfUseCase,
            IAtualizarCadastroAlunoUseCase atualizarEmailUseCase,
            IAtualizarSenhaUseCase atualizarSenhaUseCase,
            IRemoverAlunoUseCase removerAlunoUseCase)
        {
            _alunoValidator = cadastrarAlunoValidator ?? throw new ArgumentNullException(nameof(cadastrarAlunoValidator));
            _cadastrarAlunoUseCase = cadastrarAlunoUseCase ?? throw new ArgumentNullException(nameof(cadastrarAlunoUseCase));
            _obterAlunoPorNomeUseCase = obterAlunoPorNomeUseCase ?? throw new ArgumentNullException(nameof(obterAlunoPorNomeUseCase));
            _obterAlunoPorCpfUseCase = obterAlunoPorCpfUseCase ?? throw new ArgumentNullException(nameof(obterAlunoPorCpfUseCase));
            _atualizarCadastroAlunoUseCase = atualizarEmailUseCase ?? throw new ArgumentNullException(nameof(atualizarEmailUseCase));
            _atualizarSenhaUseCase = atualizarSenhaUseCase ?? throw new ArgumentNullException(nameof(atualizarSenhaUseCase));
            _removerAlunoUseCase = removerAlunoUseCase ?? throw new ArgumentNullException(nameof(removerAlunoUseCase));
        }

        [HttpPost("/cadastrar-aluno")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarAluno([FromBody] AlunoRequestDto request)
        {
            await _alunoValidator.ValidateAsync(request);

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

        [HttpPut("/aluno/atualizar-cadastro")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarCadastro([FromBody] AlunoRequestDto request)
        {
            await _alunoValidator.ValidateAsync(request);

            try
            {
                var alunoAtualizado = await _atualizarCadastroAlunoUseCase.ExecuteAsync(request);

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

        [HttpPatch("/aluno/id/{id}/atualizar-senha")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarSenha([FromRoute] int id, [FromBody] string novaSenha)
        {
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

        [HttpDelete("/aluno/id/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoverAluno([FromRoute] int id)
        {
            try
            {
                await _removerAlunoUseCase.ExecuteAsync(id);

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
