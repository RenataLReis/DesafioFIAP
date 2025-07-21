using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.Application.Dtos.Matricula
{
    public class MatriculaDto
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativa { get; set; }
        public int AlunoId { get; set; }
        public string AlunoNome { get; set; }
        public int TurmaId { get; set; }
        public string NomeTurma { get; set; }
    }
}
