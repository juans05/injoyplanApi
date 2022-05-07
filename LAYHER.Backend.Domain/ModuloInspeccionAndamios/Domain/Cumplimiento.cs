using System;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class Cumplimiento
    {
        public int Cumplimiento_id { get; set; }
        public int CheckList_id { get; set; }
        public int PreguntaInspeccionAndamio_id { get; set; }
        public int RespuestaCumplimiento_id { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public bool Activo { get; set; }
    }
}
