namespace Secretaria.Application.Services
{
    public interface IGeradorNumeroMatricula
    {
        string Gerar(string ultimaMatriculaAno, string ano);
    }
}
