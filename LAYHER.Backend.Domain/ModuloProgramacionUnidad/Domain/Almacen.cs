using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain
{
    public class Almacen
    {
        public string AlmacenCodigo { get; set; }
        public string MultiCompaniaFlag { get; set; }
        public string CompaniaSocio { get; set; }
        public string DescripcionLocal { get; set; }
        public string DescripcionIngles { get; set; }
        public string Direccion { get; set; }
        public string UnidadNegocio { get; set; }
        public string TipoAlmacen { get; set; }
        public string AlmacenTransitoPrincipal { get; set; }
        public string AlmacenVentaFlag { get; set; }
        public string AlmacenProduccionFlag { get; set; }
        public string SubAlmacenFlag { get; set; }
        public string CuentaInventario { get; set; }
        public int? NumeroMesesStock { get; set; }
        public int? Encargado { get; set; }
        public string Estado { get; set; }
        public string UltimoUsuario { get; set; }
        public DateTime? UltimaFechaModif { get; set; }
        public byte[] timestamp { get; set; }
        public string CuentaGasto { get; set; }
        public int? AlmacenRegion { get; set; }
        public string AlmacenConsignacionFlag { get; set; }
        public string DisponiblePlanProduccionFlag { get; set; }
        public string AlmacenIntermedioFlag { get; set; }
        public string AlmacenCommodityFlag { get; set; }
        public int? ComercialCajero { get; set; }
        public int? ComercialCobrador { get; set; }
        public string EstablecimientoFiscal { get; set; }
        public string EstablecimientoCodigo { get; set; }
        public string Telefono { get; set; }
        public string EstablecimientoCodigoSunat { get; set; }
        public bool FlagUsoKardex { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }
}
