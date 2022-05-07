using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloFacturacion.DTO
{
    public class VentaDetalle
    {
        public int NroDias { get; set; }
        public int NroPersonas { get; set; }


        public int Item { get; set; }

        public string CompaniaSocio { get; set; }


        public string TipoDocumento { get; set; }


        public string NumeroDocumento { get; set; }


        public int Linea { get; set; }

        public string TipoDetalle { get; set; }

        public string ItemCodigo { get; set; }

        public string Lote { get; set; }

        public string Descripcion { get; set; }

        public string UnidadCodigo { get; set; }

        public double CantidadPedida { get; set; }

        public double CantidadEntregada { get; set; }

        public string UnidadCodigoDoble { get; set; }

        public double CantidadPedidaDoble { get; set; }

        public double CantidadEntregadaDoble { get; set; }

        public double PrecioUnitario { get; set; }

        public double PrecioUnitarioFinal { get; set; }

        public double Monto { get; set; }
        public double montoTotalIgv { get; set; }

        public double MontoFinal { get; set; }

        public double MontoInteres { get; set; }

        public string IGVExoneradoFlag { get; set; }

        public string DespachoUnidadEquivalenteFlag { get; set; }

        public string ImprimirPUFlag { get; set; }

        public string AlmacenCodigo { get; set; }

        public string FlujodeCaja { get; set; }

        public string Estado { get; set; }

        public string UltimoUsuario { get; set; }

        public DateTime UltimaFechaModif { get; set; }

        public string PrecioModificadoFlag { get; set; }

        public Nullable<int> AutorizacionNumero { get; set; }

        public DateTime RutaDespachoFechaAsignacion { get; set; }

        public string RutaDespachoPlacaVehiculo { get; set; }

        public Nullable<int> Agro_Tienda { get; set; }

        public string ExportacionMarcaPaquete { get; set; }

        public double ExportacionCantidadEmbarque { get; set; }

        public DateTime ExportacionFechaProgramacion { get; set; }

        public string ExportacionComentarios { get; set; }

        public string ExportacionSituacion { get; set; }

        public double PrecioUnitarioDoble { get; set; }

        public string Condicion { get; set; }

        public string ReferenciaFiscal02 { get; set; }

        public double PrecioUnitarioOriginal { get; set; }

        public string NumeroSerie { get; set; }

        public string TransferenciaGratuitaFlag { get; set; }

        public double PrecioUnitarioGratuito { get; set; }

        public double PorcentajeDescuento01 { get; set; }

        public double PorcentajeDescuento02 { get; set; }

        public double PorcentajeDescuento03 { get; set; }

        public string DocumentoRelacTipoDocumento { get; set; }

        public string DocumentoRelacNumeroDocumento { get; set; }

        public Nullable<int> DocumentoRelacLinea { get; set; }

        public string CentroCosto { get; set; }

        public string Proyecto { get; set; }
        public string vc_proyecto { get; set; }

        public string OrigenCotizacionFlag { get; set; }

        public double MontoDescuento01 { get; set; }

        public double MontoDescuento02 { get; set; }

        public double MontoDescuento03 { get; set; }

        public double in_CantFaltante { get; set; }
        public double Cantidad { get; set; }
        public string vc_Producto { get; set; }
        public int sw_ProdDespachado { get; set; }
        public string vc_CodCorrelativo { get; set; }

        public int in_CantNuevo { get; set; }
        public int in_CantUsado { get; set; }
        public string correlativo { get; set; }
        public int in_CantCte { get; set; }

        // OC para AB
        public string OrdenCompra { get; set; }

        // EQUIPOS LIQ FACTURADOS

        public string FechaDocumento { get; set; }
        public string vc_TipoSolVta { get; set; }
        public string MontoAfecto { get; set; }
        public string MontoImpuestoVentas { get; set; }
        public string MontoDescuentos { get; set; }
        public string MontoTotal { get; set; }

        public int in_Cantidad_equipos_facturados { get; set; }
    }
}
