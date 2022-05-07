using System;


namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class PreguntaInspeccion
    {
        public int PreguntaInspeccionAndamio_id { get; set; }
        public int TipoAndamio_id { get; set; }
        public int SubTipoAndamio_id { get; set; }
        public int Orden { get; set; }
        public string PreguntaInspeccionAndamio { get; set; }
        public bool Activo { get; set; }

        public Cumplimiento Cumplimiento { get; set; }
    }
}
