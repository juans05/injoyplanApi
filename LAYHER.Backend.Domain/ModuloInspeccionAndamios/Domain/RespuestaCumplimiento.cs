using System;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class Respuesta
    {
        public int RespuestaCumplimiento_id { get; set; }
        public string RespuestaCumplimiento { get; set; }
    }

    public enum ERespuestaCumplimiento
    {
        Si = 1,
        No = 2,
        NA = 3,
    }
}
