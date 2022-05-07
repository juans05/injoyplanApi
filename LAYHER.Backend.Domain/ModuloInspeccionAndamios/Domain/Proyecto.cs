using System;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class Proyecto
    {
        public string Proyecto_id { get; set; }
        public string Nombre { get; set; }
        public int Cliente_id { get; set; }
        public string NroDocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
    }
}
