using Secretaria.Application.Dtos.Turma;

namespace Secretaria.Application.Interfaces.Turma.Commands
{
    public interface ICriarTurmaUseCase
    {
        Task<TurmaDto> ExecuteAsync(TurmaRequestDto turmaDto);
    }
}
