using System;
using System.Collections.Generic;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class CheckList
    {
        public int CheckList_id  { get; set; }
        public int InspeccionAndamio_id  { get; set; }
        public int TipoAndamio_id  { get; set; }
        public string TipoAndamio_Nombre { get; set; }
        public string SubTipoAndamio_Nombre { get; set; }
        public int? SubTipoAndamio_id  { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public bool Activo { get; set; }
        public string strMode { get; set; }
        public int IdUsuario { get; set; }

        public List<PreguntaInspeccion> ListaPreguntaInspeccion { get; set; }
    }
}
