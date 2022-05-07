using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class Consideracion
    {
        public int IdConsideracion { get; set; }
        public string NombreConsideracion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class FormularioConsideracion
    {
        public int IdFormulario { get; set; }
        public int IdConsideracion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdiciom { get; set; }
        public DateTime FechaEdicion { get; set; }
    }
}
