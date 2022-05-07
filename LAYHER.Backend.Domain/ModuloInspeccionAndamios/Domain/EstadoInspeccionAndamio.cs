using System;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class EstadoInspeccionAndamio
    {
        public int EstadoInspeccionAndamio_id  { get; set; }
        public string Nombre  { get; set; }
    }
    public enum EEstadoInspeccionAndamio
    {
        Borrador = 1,
        Completado = 2,
        Historico = 3,
    }
}
