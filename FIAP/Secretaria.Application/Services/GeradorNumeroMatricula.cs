namespace Secretaria.Application.Services
{
    public class GeradorNumeroMatricula : IGeradorNumeroMatricula
    {
        public string Gerar(string ultimaMatriculaAno, string ano)
        {
            var quantidadeMatriculasAno = int.TryParse(ultimaMatriculaAno, out var ultimoNumero) ? ultimoNumero : 0;
            var sequencial = quantidadeMatriculasAno + 1;

            return $"{ano}{sequencial:D5}";
        }
    }
}
