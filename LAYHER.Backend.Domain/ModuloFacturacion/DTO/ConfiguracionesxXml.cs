using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloFacturacion.DTO
{
    public class ConfiguracionesxXml
    {
        public string CodigoOperacion { get; set; }
        public string CodigoOperacion2 { get; set; }
        public string CodigoOperacion3 { get; set; }
        public string IdentificacionEmisor { get; set; }
        public string TipoDocumentoEmisor { get; set; }
        public string SitioWebEmisor { get; set; }
        public string TelefonoEmisor { get; set; }
        public string NombreEmisor { get; set; }
        public string CodigoUbigeoEmisor { get; set; }
        public string DireccionEmisor { get; set; }
        public string UrbanizacionEmisor { get; set; }
        public string ProvinciaEmisor { get; set; }
        public string DepartamentoEmisor { get; set; }
        public string DistritoEmisor { get; set; }
        public string CodigoPaisEmisor { get; set; }
        public string RazonSocialEmisor { get; set; }
        public string MedioDePago { get; set; }
        public string CuentaBanco { get; set; }
        public string CodigoDetraccion { get; set; }
        public string PorcentajeDetraccion { get; set; }
        public string MonedaDetraccion { get; set; }
        public string CodigoLeyenda { get; set; }
        public string CodigoLeyenda2 { get; set; }
        public string TextoLeyenda2 { get; set; }

        public string RutaGeneracionXMLs { get; set; }
        public string RutaArchivoCorrelativoAnulacionFactura { get; set; }
        public string RutaArchivoCorrelativoAnulacionBoleta { get; set; }
        public string ConnectionString { get; set; }
        public string QueryDocumento { get; set; }
        public string QueryDetalles { get; set; }
        public string QueryDocumentoAsocidado { get; set; }
        public string QueryContrato { get; set; }
        public string QueryVendedor { get; set; }
        public string QueryCliente { get; set; }

        public string QueryEncabezadoGuiaRemision { get; set; }
        public string QueryDetalleGuiaRemision { get; set; }
        public string QueryEncabezadoAdicionalGuiaRemision { get; set; }
        public string QueryMotivoTrasladoGuiarRemision { get; set; }

        public string QueryAnulacionFactura { get; set; }
        public string QueryAnulacionBoleta { get; set; }
        public string QueryDocumentoRelacionadoNotaAnulacion { get; set; }
    }
}
