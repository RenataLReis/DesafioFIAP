using Secretaria.Application.Dtos.Turma;

namespace Secretaria.Application.Interfaces.Turma.Commands
{
    public interface IAtualizarTurmaUseCase
    {
        Task<TurmaDto> ExecuteAsync(TurmaRequestDto turmaDto);
    }
}
