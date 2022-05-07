using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class ProgramacionTiempo
    {
        public int IdProgramacionTiempo { get; set; }
        public int IdProgramacionUnidad { get; set; }
        public int IdTipoMontacarga { get; set; }
        public DateTime DescargaInicio { get; set; }
        public DateTime DescargaFin { get; set; }
        public DateTime FechaRevision { get; set; }
        public DateTime InicioRevision { get; set; }
        public DateTime FinRevision { get; set; }
        public DateTime FechaRevision2 { get; set; }
        public DateTime InicioRevision2 { get; set; }
        public DateTime FinRevision2 { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
        public bool RevisionMayorUno { get; set; }
    }
}
