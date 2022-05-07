using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class Metraje
    {
        public int IdMetrajeTrailer { get; set; }
        public string Metro { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
    }
}
