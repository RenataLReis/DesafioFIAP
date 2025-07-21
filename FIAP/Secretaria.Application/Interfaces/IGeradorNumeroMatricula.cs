namespace Secretaria.Application.Interfaces
{
    public interface IGeradorNumeroMatricula
    {
        string Gerar(string ultimaMatriculaAno, string ano);
    }
}
