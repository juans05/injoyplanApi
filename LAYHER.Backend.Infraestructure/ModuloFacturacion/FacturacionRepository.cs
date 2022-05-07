using Dapper;
using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloFacturacion.DTO;
using LAYHER.Backend.Domain.ModuloFacturacion.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloFacturacion
{
    public class FacturacionRepository : IFacturacionRepository
    {

        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<FacturacionRepository> _logger;
        public FacturacionRepository(ICustomConnection _connection, ILogger<FacturacionRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }
        public async Task<StatusReponse<VentaCabecera>> SaveVentaCabecera(VentaCabecera ventaCabecera)
        {
            StatusReponse<VentaCabecera> status = new StatusReponse<VentaCabecera>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<VentaCabecera>("[COMERCIAL].[USP_INSERT_FC_VentasCabecera_SHOP]",
                    new
                    {
                        @CompaniaSocio = ventaCabecera.CompaniaSocio,
                        @TipoDocumento = "PE", /*solicitud de venta*/
                        @NumeroDocumento = ventaCabecera.NumeroDocumento,
                        @EstablecimientoCodigo = ventaCabecera.EstablecimientoCodigo,
                        @FormaFacturacion = ventaCabecera.FormaFacturacion,
                        @ClienteNumero = ventaCabecera.ClienteNumero,
                        @ClienteRUC = ventaCabecera.ClienteRUC,
                        @ClienteNombre = ventaCabecera.ClienteNombre,
                        @ClienteDireccion = ventaCabecera.ClienteDireccion,
                        @ClienteCobrarA = ventaCabecera.ClienteCobrarA,
                        @FechaDocumento = ventaCabecera.FechaDocumento,
                        @FechaVencimiento = ventaCabecera.FechaVencimiento,
                        @TipoFacturacion = ventaCabecera.TipoFacturacion,
                        @TipoVenta = ventaCabecera.TipoVenta,
                        @ConceptoFacturacion = "SER",
                        @FormadePago = ventaCabecera.FormadePago,
                        @Vendedor = ventaCabecera.Vendedor,
                        @TipodeCambio = ventaCabecera.TipodeCambio,
                        @MonedaDocumento = ventaCabecera.MonedaDocumento,
                        @MontoAfecto = ventaCabecera.MontoAfecto,
                        @MontoNoAfecto = ventaCabecera.MontoNoAfecto,
                        @MontoImpuestoVentas = ventaCabecera.MontoImpuestoVentas,
                        @MontoImpuestos = ventaCabecera.MontoImpuestos,
                        @MontoDescuentos = ventaCabecera.MontoDescuentos,
                        @MontoPagado = ventaCabecera.MontoPagado,
                        @PreparadoPor = ventaCabecera.PreparadoPor,
                        @FechaPreparacion = DateTime.Now,
                        @AlmacenCodigo = ventaCabecera.AlmacenCodigo,
                        @CentroCosto = ventaCabecera.CentroCosto,
                        @UltimoUsuario = ventaCabecera.UltimoUsuario,
                        @UltimaFechaModif = ventaCabecera.UltimaFechaModif,
                        @Estado = ventaCabecera.Estado,
                        @UnidadReplicacion = ventaCabecera.UnidadReplicacion,
                        @Proyecto = ventaCabecera.Proyecto,
                        @Comentarios = ventaCabecera.Comentarios,
                        @TipoFormadePago = ventaCabecera.TipoFormadePago,
                        @TipoListaPrecio = ventaCabecera.TipoListaPrecio,
                        @Criteria = ventaCabecera.Criteria,
                        @ImpresionPendienteFlag = ventaCabecera.ImpresionPendienteFlag,
                        @DocumentoMoraFlag = ventaCabecera.DocumentoMoraFlag,
                        @ContabilizacionPendienteFlag = ventaCabecera.ContabilizacionPendienteFlag,
                        @VoucherPeriodo = ventaCabecera.VoucherPeriodo,
                        @Sucursal = ventaCabecera.Sucursal,
                        @TipoCanjeFactura = ventaCabecera.TipoCanjeFactura,
                        @LetraCarteraFlag = ventaCabecera.LetraCarteraFlag,
                        @LetraDescuentoIntereses = ventaCabecera.LetraDescuentoIntereses,
                        @LetraDescuentoVoucherFlag = ventaCabecera.LetraDescuentoVoucherFlag,
                        @APTransferidoFlag = ventaCabecera.APTransferidoFlag,
                        @UnidadNegocio = ventaCabecera.UnidadNegocio,
                        @ComentariosImprimirFlag = ventaCabecera.ComentariosImprimirFlag,
                        @ComentariosMonto = ventaCabecera.ComentariosMonto,
                        @RutaDespacho = ventaCabecera.RutaDespacho,
                        @MontoRedondeo = ventaCabecera.MontoRedondeo,
                        @ClienteDireccionSecuencia = ventaCabecera.ClienteDireccionSecuencia,
                        @VentaCondicionalFlag = ventaCabecera.VentaCondicionalFlag,
                        @CentralRiesgoFlag = ventaCabecera.CentralRiesgoFlag,
                        @SerieItemenFacturaFlag = ventaCabecera.SerieItemenFacturaFlag,
                        @IngresoSeriesxVentasFlag = ventaCabecera.IngresoSeriesxVentasFlag,
                        @Interes = ventaCabecera.Interes,
                        @MontoAfectoG = ventaCabecera.MontoAfectoG,
                        @MontoNoAfectoG = ventaCabecera.MontoNoAfectoG,
                        @montoimpuestoventasG = ventaCabecera.montoimpuestoventasG,
                        @vc_idPedidoAlq = ventaCabecera.vc_idPedidoAlq,
                        @vc_TipoSolVta = ventaCabecera.vc_TipoSolVta,
                        @dc_Adelanto = ventaCabecera.dc_Adelanto,
                        @dc_RentaMin = ventaCabecera.dc_RentaMin,
                        @Periodo = DateTime.Now.ToString(),
                        @Monto_NC = ventaCabecera.@Monto_NC,
                        @vc_RefDocRe_facturacion = ventaCabecera.vc_RefDocRe_facturacion,
                        @vc_RefDocumento_NFacturas = ventaCabecera.vc_RefDocumento_NFacturas,
                        @MontoTotal = ventaCabecera.MontoTotal,
                        @nroGuiaShop = ventaCabecera.nroGuiaShop
                    }, commandType: CommandType.StoredProcedure);
                    this._logger.LogInformation("OK");
                    status.Data = (VentaCabecera)items.FirstOrDefault();

                    status.Success = true;
                }
                catch (Exception e)
                {
                    this._logger.LogError(e, "SaveVentaCabecera : Error");
                    status.Success = false;
                }
            }
            return status;
        }

        public async Task<StatusReponse<VentaDetalle>> SaveVentaDetalle(VentaDetalle ventaDetalles)
        {
            StatusReponse<VentaDetalle> status = new StatusReponse<VentaDetalle>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<VentaDetalle>("[COMERCIAL].[USP_INSERT_FC_VentasDetalle]",
                    new
                    {
                        @CompaniaSocio = ventaDetalles.CompaniaSocio,
                        @TipoDocumento = ventaDetalles.TipoDocumento,
                        @NumeroDocumento = ventaDetalles.NumeroDocumento,
                        @Linea = ventaDetalles.Linea,
                        @TipoDetalle = ventaDetalles.TipoDetalle,
                        @ItemCodigo = ventaDetalles.ItemCodigo,
                        @Descripcion = ventaDetalles.Descripcion,
                        @CantidadPedida = ventaDetalles.CantidadPedida,
                        @PrecioUnitario = ventaDetalles.PrecioUnitario,
                        @PrecioUnitarioFinal = ventaDetalles.PrecioUnitarioFinal,
                        @Monto = ventaDetalles.Monto,
                        @MontoFinal = ventaDetalles.MontoFinal,
                        @IGVExoneradoFlag = ventaDetalles.IGVExoneradoFlag,
                        @AlmacenCodigo = ventaDetalles.AlmacenCodigo,
                        @Estado = ventaDetalles.Estado,
                        @UltimoUsuario = ventaDetalles.UltimoUsuario,
                        @UltimaFechaModif = ventaDetalles.UltimaFechaModif,
                        @CentroCosto = ventaDetalles.CentroCosto,
                        @PrecioUnitarioOriginal = ventaDetalles.PrecioUnitarioOriginal,
                        @UnidadCodigo = ventaDetalles.UnidadCodigo,
                        @CantidadEntregada = ventaDetalles.CantidadEntregada,
                        @CantidadPedidaDoble = ventaDetalles.CantidadPedidaDoble,
                        @CantidadEntregadaDoble = ventaDetalles.CantidadEntregadaDoble,
                        @MontoInteres = ventaDetalles.MontoInteres,
                        @DespachoUnidadEquivalenteFlag = ventaDetalles.DespachoUnidadEquivalenteFlag,
                        @ImprimirPUFlag = ventaDetalles.ImprimirPUFlag,
                        @FlujodeCaja = ventaDetalles.FlujodeCaja,
                        @PrecioModificadoFlag = ventaDetalles.PrecioModificadoFlag,
                        @PrecioUnitarioDoble = ventaDetalles.PrecioUnitarioDoble,
                        @Condicion = ventaDetalles.Condicion,
                        @TransferenciaGratuitaFlag = ventaDetalles.TransferenciaGratuitaFlag,
                        @PrecioUnitarioGratuito = ventaDetalles.PrecioUnitarioGratuito,
                        @PorcentajeDescuento01 = ventaDetalles.PorcentajeDescuento01,
                        @PorcentajeDescuento02 = ventaDetalles.PorcentajeDescuento02,
                        @PorcentajeDescuento03 = ventaDetalles.PorcentajeDescuento03,
                        @OrigenCotizacionFlag = ventaDetalles.OrigenCotizacionFlag,
                        @MontoDescuento01 = ventaDetalles.MontoDescuento01,
                        @MontoDescuento02 = ventaDetalles.MontoDescuento02,
                        @MontoDescuento03 = ventaDetalles.MontoDescuento03,
                        @Proyecto = ventaDetalles.Proyecto
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (VentaDetalle)items.FirstOrDefault();
                    this._logger.LogInformation("OK : SaveVentaDetalle");
                    status.Success = true;
                }
                catch (Exception e)
                {
                    this._logger.LogError(e, "SaveVentaCabecera : Error");
                    status.Success = false;
                }
            }
            return status;
        }
    }
}
