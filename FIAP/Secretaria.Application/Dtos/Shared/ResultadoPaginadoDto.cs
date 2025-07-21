namespace Secretaria.Application.Dtos.Shared
{
    public class ResultadoPaginadoDto<T>
    {
        public IEnumerable<T> Itens { get; set; } = new List<T>();
        public int Pagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalItens { get; set; }
    }
}
