namespace Secretaria.Application.Interfaces.Aluno.Commands
{
    public interface IAtualizarSenhaUseCase
    {
        Task ExecuteAsync(int alunoId, string novaSenha);
    }
}
