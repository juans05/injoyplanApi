using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloFacturacion.DTO
{
    public class VentaCabecera
    {
        public decimal Deuda { get; set; }
        public decimal FactorVenta { get; set; }
        public string Tipo_Documento { get; set; }
        public string cobranzaNumero { get; set; }
        public string DNI { get; set; }
        public double dbl_montoFactura { get; set; }
        public double MONTOLOCAL { get; set; }
        public double dbl_montoPagado { get; set; }
        public double dbl_montoNC { get; set; }
        public double dbl_montoND { get; set; }
        public string FC_NumeroDocumento { get; set; }
        public string Numero_Documento { get; set; }
        public string nombreCliente { get; set; }
        public string CambioCliente { get; set; }
        public string usCambioCliente { get; set; }
        public decimal tipoCambio { get; set; }
        public string CLIENTE { get; set; }
        public string Monto_Facturado { get; set; }
        public string Monto_Pagado { get; set; }
        public string exoneradoImpuesto { get; set; }
        public string Monto_Saldo { get; set; }
        public string Cliente_RUC { get; set; }
        public string Tipo_Sol_Vta { get; set; }
        public string Fecha_Comprobante { get; set; }
        public string Num_Ref_NC_ND { get; set; }
        public string Comercial { get; set; }
        public string NombreComercial { get; set; }
        public string Telefono { get; set; }
        public string NroGuia { get; set; }
        public string SimboloMoneda { get; set; }
        public string IGV { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumBooking { get; set; }
        public string Barco { get; set; }
        public string DescripcionLarga { get; set; }
        public string DocumentoFiscal { get; set; }
        public string DireccionComun { get; set; }
        public string vc_pais { get; set; }
        public string vc_departamento { get; set; }
        public string vc_provincia { get; set; }
        public string vc_distrito { get; set; }
        public string MonedaDocumento2 { get; set; }
        public int id_ejecutivo { get; set; }
        public string observaciones { get; set; }
        public string TipoDocumentoSunat { get; set; }
        public string Electronico { get; set; }
        public string TipDocEmis { get; set; }
        public string vc_MonedaFacElect { get; set; }
        public string DepartamentoEmisor { get; set; }
        public string ProvinciaEmisor { get; set; }
        public string DistritoEmisor { get; set; }
        public string MotivoAnular { get; set; }
        public string PeriodoValorizacion { get; set; }

        public string FlagDespacho { get; set; }


        public string CompaniaSocio { get; set; }
        public string Parametro { get; set; }
        public string TipoDocumento { get; set; }
        public string FINANCIERA { get; set; }
        public string MODOPAGO { get; set; }

        public string NumeroDocumento { get; set; }
        public string EstablecimientoCodigo { get; set; }
        public string codigoCliente { get; set; }
        public string ddlFinanciero { get; set; }


        public string FormaFacturacion { get; set; }

        public int ClienteNumero { get; set; }

        public string ClienteRUC { get; set; }

        public string ClienteNombre { get; set; }

        public string ClienteDireccion { get; set; }

        public string ClienteReferencia { get; set; }

        public int ClienteCobrarA { get; set; }

        public DateTime FechaDocumento { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public DateTime Fecha { get; set; }
        public string dt_FechaCobranza { get; set; }

        public string FENTREGA { get; set; }

        public string TipoFacturacion { get; set; }

        public string TipoVenta { get; set; }

        public string ConceptoFacturacion { get; set; }

        public string FormadePago { get; set; }

        public string VC_FORMADEPAGO { get; set; }

        public string VC_VENDEDOR { get; set; }

        public string Criteria { get; set; }

        // Public Overridable Property Vendedor As Integer

        public double TipodeCambio { get; set; }
        public double montoImporte { get; set; }

        public string MonedaDocumento { get; set; }

        public double MontoAfecto { get; set; }
        public double Pago { get; set; }
        public double montoRealSistema { get; set; }



        public double MontoNoAfecto { get; set; }

        public double MontoImpuestoVentas { get; set; }

        public double MontoImpuestos { get; set; }

        public double MontoDescuentos { get; set; }

        public double MontoTotal { get; set; }
        public double SubTotal { get; set; }

        public double MontoPagado { get; set; }

        public double MontoAdelantoSaldo { get; set; }

        public int PreparadoPor { get; set; }

        public int AprobadoPor { get; set; }

        public int RechazadoPor { get; set; }

        public DateTime FechaPreparacion { get; set; }

        public DateTime FechaAprobacion { get; set; }

        public DateTime FechaRechazo { get; set; }

        public string MotivoRechazo { get; set; }

        public string NotaCreditoDocumento { get; set; }

        public string ComercialPedidoNumero { get; set; }

        public DateTime ComercialPedidoFechaRequerida { get; set; }

        public string AlmacenCodigo { get; set; }

        public string ImpresionPendienteFlag { get; set; }

        public string DocumentoMoraFlag { get; set; }

        public string ContabilizacionPendienteFlag { get; set; }

        public string VoucherPeriodo { get; set; }

        public string VoucherNo { get; set; }

        public string VoucherAnulacion { get; set; }

        public string CentroCosto { get; set; }

        public string Proyecto { get; set; }

        public string CampoReferencia { get; set; }

        public string Sucursal { get; set; }

        public string TipoCanjeFactura { get; set; }

        public string LetraCarteraFlag { get; set; }

        public string LetraBanco { get; set; }

        public string LetraCobranzaNumero { get; set; }

        public string LetraAvalNombre { get; set; }

        public string LetraAvalRUC { get; set; }

        public string LetraAvalDireccion { get; set; }

        public string LetraAvalTelefono { get; set; }

        public string LetraAceptadoPor { get; set; }

        public string LetraUbicacion { get; set; }

        public DateTime LetraFechaRecepcion { get; set; }

        public string LetraDescuentoCuentaBancaria { get; set; }

        public double LetraDescuentoIntereses { get; set; }

        public string LetraDescuentoVoucherFlag { get; set; }

        public string LetraDescuentoVoucher { get; set; }

        public string LetraDescuentoCanjeFlag { get; set; }

        public string LetraDescuentoCanjeVoucher { get; set; }

        public string DescuentoFlag { get; set; }

        public string APTransferidoFlag { get; set; }

        public int APProcesoNumero { get; set; }

        public int APProcesoSecuencia { get; set; }

        public string CobranzaDudosaEstado { get; set; }

        public string CobranzaDudosaVoucher { get; set; }

        public string CobranzaDudosaVoucherClearing { get; set; }

        public DateTime CobranzaDudosaFecha { get; set; }

        public DateTime CobranzaDudosaFechaClearing { get; set; }

        public string UnidadNegocio { get; set; }

        public string UnidadReplicacion { get; set; }

        public string ProcesoImportacion { get; set; }

        public string ProcesoImportacionNumero { get; set; }

        public DateTime ProcesoImportacionFecha { get; set; }

        public string Comentarios { get; set; }

        public string ComentariosImprimirFlag { get; set; }

        public double ComentariosMonto { get; set; }

        public string NumeroInterno { get; set; }

        public string RutaDespacho { get; set; }

        public string Estado { get; set; }

        public string UltimoUsuario { get; set; }

        public DateTime UltimaFechaModif { get; set; }

        public string NotadeCreditoFlag { get; set; }

        public double MontoRedondeo { get; set; }

        public string LetraAceptadoFlag { get; set; }

        public string ClienteContacto { get; set; }

        public int ClienteDireccionSecuencia { get; set; }

        public string TransportistaChofer { get; set; }

        public string TransportistaVehiculo { get; set; }

        public string ReprogramacionMotivo { get; set; }

        public string ReprogramacionPuntoPartida { get; set; }

        public string ReprogramacionPuntoLlegada { get; set; }

        public DateTime APFechaTransferencia { get; set; }

        public string DocumentosinDespachoFlag { get; set; }

        public string DocumentosinCantidadFlag { get; set; }

        public DateTime RutaDespachoFechaAsignacion { get; set; }

        public string RutaDespachoPlacaVehiculo { get; set; }

        public int Cobrador { get; set; }

        public string LetraProtestoFlag { get; set; }

        public int AprobadoPorDescuento { get; set; }

        public DateTime FechaVencimientoOriginal { get; set; }

        public string NumeroContrato { get; set; }
        public string Usuario { get; set; }

        public string CentroCostoDestino { get; set; }

        public string CobranzaDudosaMotivo { get; set; }

        public double MontoImpuestoRetenido { get; set; }

        public string DiferidoFlag { get; set; }

        public string APTransferidoFlagCRM { get; set; }

        public double TransferenciaGratuitaIGVFactor { get; set; }

        public double ComisionTipoCambio { get; set; }

        public double ComisionPorcentajeOriginal { get; set; }

        public double ComisionPorcentaje { get; set; }

        public double ComisionMontoBase { get; set; }

        public double ComisionMontoTotal { get; set; }

        public double ComisionImporte { get; set; }

        public string VentaCondicionalFlag { get; set; }

        public string VentaCondicionalObs { get; set; }

        public DateTime VentaCondicionalFecha { get; set; }

        public double VentaCondicionalImporte { get; set; }

        public double MargenPorcentaje { get; set; }

        public double MargenImporte { get; set; }

        public string DocumentoSituacion { get; set; }

        public string CentralRiesgoFlag { get; set; }

        public string CentralRiesgo { get; set; }

        public double CentralRiesgoMonto { get; set; }

        public DateTime CentralRiesgoFecha { get; set; }

        public string CentralRiesgoUsuario { get; set; }

        public string ProcesoJudicialFlag { get; set; }

        public string ProcesoJudicialNumero { get; set; }

        public DateTime DeudaFechaCalculo { get; set; }

        public double DeudaTotalaPagar { get; set; }

        public double DeudaMontoCobrado { get; set; }

        public string DeudaCuotaVencida { get; set; }

        public int DeudaDiasMora { get; set; }

        public DateTime DeudaFechaPrometida { get; set; }

        public double DeudaDeudaVencida { get; set; }

        public string RefinanciamientoFlag { get; set; }

        public string RefinanciamientoNumero { get; set; }

        public string TipoFormadePago { get; set; }

        public string TipoListaPrecio { get; set; }

        public int Cajero { get; set; }

        public DateTime FechaImpresion { get; set; }

        public string DiferidoDocumento { get; set; }

        public string SerieItemenFacturaFlag { get; set; }

        public string IngresoSeriesxVentasFlag { get; set; }

        public string TipoConvenio { get; set; }

        public string CuotaInicialFlag { get; set; }

        public string EstadoCredito { get; set; }

        public DateTime FechaPreparacionCred { get; set; }

        public int PreparadoPorCred { get; set; }

        public DateTime FechaAprobacionCred { get; set; }

        public int AprobadoPorCred { get; set; }

        public DateTime FechaRevisionCred { get; set; }

        public int RevisadoPorCred { get; set; }

        public double Interes { get; set; }

        public double MontoConInteres { get; set; }

        public DateTime FechaRevision { get; set; }

        public int RevisadoPor { get; set; }

        public DateTime FechaUltimoPago { get; set; }

        public string FlagMora { get; set; }

        public double Mora { get; set; }

        public double ProntoPago { get; set; }

        public string UsuarioModifMora { get; set; }

        public DateTime FechaModifMora { get; set; }

        public string MoraComentario { get; set; }

        public string MotivoDevolucion { get; set; }

        public string GeneracionAutomaticaFlag { get; set; }

        public string RegistroMigradoFlag { get; set; }

        public double MontoAfectoG { get; set; }

        public double MontoNoAfectoG { get; set; }

        public double montoimpuestoventasG { get; set; }

        public string Obs_Modelo { get; set; }

        public string Obs_Placa { get; set; }


        public string VC_MONEDA { get; set; }
        public string VC_FormaFacturacion { get; set; }
        public string VC_TIPODOC { get; set; }
        public string VC_ESTADO { get; set; }
        public string VC_CODCOMPUESTO { get; set; }
        public string VC_NAME_PROYECTO { get; set; }
        public string VC_NAMECOMPANY { get; set; }
        public string VC_TIENDA { get; set; }
        public string VC_ALMACEN { get; set; }
        public string NumeroSerie { get; set; }
        public string Caja { get; set; }
        public string VC_LISTAPRECIO { get; set; }
        public DateTime dt_FecAccion { get; set; }

        public string vc_idCotizacCompuesto { get; set; }
        public string vc_idPedidoAlq { get; set; }

        // REM LEONARDO
        public string Busqueda { get; set; }
        public string Clasificacion { get; set; }
        public double MontoPendiente { get; set; }

        public string vc_TipoSolVta { get; set; }
        public double dc_Adelanto { get; set; }

        public string vc_idProyecto { get; set; }
        public string vc_Proyecto { get; set; }
        public string vc_DescTipoSolVta { get; set; }
        public string vc_RUC { get; set; }
        public string vc_NroFactura { get; set; }
        public string vc_DirecProyecto { get; set; }
        public string FC_ENTREGA { get; set; }
        public string vc_NroSerie { get; set; }
        public string vc_Direccion { get; set; }

        public int TOTAL_DIAS { get; set; }

        public string vc_NroSolVta { get; set; }
        public string PERIODO { get; set; }
        public string vc_idContrato { get; set; }

        public int in_Sub_Diario { get; set; }
        public string vc_Tipo { get; set; }

        public string Vendedor { get; set; }
        public string MontoLetras { get; set; }
        // validacion
        public int valor { get; set; }
        public string NroFacturacion { get; set; }
        public double dc_RentaMin { get; set; }
        // OC AB
        public string OrdenCompra { get; set; }

        // Detraccion
        public string PorcentajeDetraccion { get; set; }
        public double MontoDetraccion { get; set; }

        // REM LEONARDO
        public string vc_DesTipoSolVta { get; set; }
        public string Documento { get; set; }
        public string vc_RefDocRe_facturacion { get; set; }

        // ADD SERIE MAESTRO FACTURA
        public string Serie { get; set; }
        public string prueba { get; set; }
        public string VC_NOMTIPODOCUMENTO { get; set; }
        public string vc_idCotizaCompuesto { get; set; }
        public string TotalAdelantos { get; set; }
        public string dt_FecRecepDocCliente { get; set; }
        public int DiasVencidos { get; set; }
        public double Monto_NC { get; set; }
        public string VC_RefCobranza { get; set; }
        // ADL IQ VAL.
        public DateTime dt_FechaCreacion { get; set; }
        public decimal dc_MontoTomado { get; set; }
        public string Concepto { get; set; }
        public string Nombre_Documento { get; set; }
        public string LocalName { get; set; }
        public string vc_FechaDocumento { get; set; }
        public string vc_FecRecepDocCliente { get; set; }
        public string DescripcionLocal { get; set; }
        public string Nom_TipoDoc { get; set; }
        public string vc_DocumentoReferenciado { get; set; }
        public string vc_RefDocumento_NFacturas { get; set; }
        public string NombreCompleto { get; set; }

        // REM LEONARDO
        public string vc_idCotizacion { get; set; }
        public string vc_idPeriodo { get; set; }
        public string vc_idEstado { get; set; }

        //pre guia

        public string nroGuiaShop { get; set; }
        public List<VentaDetalle> VentaDetalles { get; set; }
    }
}
