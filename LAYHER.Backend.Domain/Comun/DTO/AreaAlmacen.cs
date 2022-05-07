using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.Comun.DTO
{
    public class AreaAlmacen
    {
        public int IdPersonaAlmacen { get; set; }
        public int IdPersona { get; set; }
        public string AlmacenCodigo { get; set; }
        public string AlmacenNombre { get; set; }
        public string Latitud { get; set; } = "12.0389421";
        public string Longitud { get; set; } = "76.8500393";
        public string Direccion { get; set; } = "Por validar dirección";

    }

    public class RegionAlmacen
    {
        public int AlmacenRegion { get; set; }
        public string DescripcionLocal { get; set; }
        public string Estado { get; set; }
    }
}
