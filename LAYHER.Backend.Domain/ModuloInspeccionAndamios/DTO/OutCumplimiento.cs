using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO
{
    public class OutCumplimiento
    {
        public int TipoAndamio_id { get; set; }
        public int? SubTipoAndamio_id { get; set; }
        public string TipoAndamio_Nombre { get; set; }
        public string SubTipoAndamio_Nombre { get; set; }

        public int CheckList_id { get; set; }


        public int Cumplimiento_id { get; set; }
        public int PreguntaInspeccionAndamio_id { get; set; }
        public int Orden { get; set; }
        public string PreguntaInspeccionAndamio { get; set; }
        public int RespuestaCumplimiento_id { get; set; }
        public string RespuestaCumplimiento { get; set; }
    }
}
