using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class TipoProgramacion
    {
        public int IdTipoProgramacion { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
    }

    public class ProgramacionEstado
    {
        public int IdTipoProgramacion { get; set; }
        public int IdTipoEstado { get; set; }
        public string Nombre { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
