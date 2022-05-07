using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Domain.ModuloFacturacion.DTO;
using LAYHER.Backend.Domain.ModuloFacturacion.Interfaces;
using LAYHER.Backend.Domain.ModuloPreguia_Guia.Interfaces;
using LAYHER.Backend.Domain.ModuloProductos.DTO;
using LAYHER.Backend.Domain.ModuloProductos.Interfaces;
using LAYHER.Backend.Domain.ModuloWebShop.DTO;
using LAYHER.Backend.Domain.ModuloWebShop.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloWebShop
{
    public class WebShopApp : BaseApp<WebShopApp>
    {
        private IWebShopRepository _webShopRepository;
        private IMaestroPersonasRepository _maestroPersonaRepository;
        private IControlCantidadesPreguiaGuiaRepository _ControlCantidadesPreguiaGuiaRepositorys;
        private IFacturacionRepository _FacturacionRepository;
        private IProductoRepository _IProductoRepository;
        public WebShopApp(IWebShopRepository webShopRepository, IMaestroPersonasRepository maestroPersonaRepository, IControlCantidadesPreguiaGuiaRepository ControlCantidadesPreguiaGuiaRepositorys, IFacturacionRepository facturacionRepository, IProductoRepository productoRepository,
                             ILogger<BaseApp<WebShopApp>> logger) : base()
        {
            this._webShopRepository = webShopRepository;
            this._maestroPersonaRepository = maestroPersonaRepository;
            this._ControlCantidadesPreguiaGuiaRepositorys = ControlCantidadesPreguiaGuiaRepositorys;
            this._FacturacionRepository = facturacionRepository;
            this._IProductoRepository = productoRepository;
        }

        //public async Task<StatusReponse<List<Eventos>>> ListProductoMarketin()
        //{
        //    StatusReponse<List<Eventos>> status = null;
        //    status = await this.ComplexResponse(() => _IProductoRepository.ListarProductos());
        //    return status;
        //}

        public async Task<StatusResponse> SavePersona(MaestroPersonas entity)
        {
            StatusReponse<MaestroPersonas> formulario = new StatusReponse<MaestroPersonas>() { Success = false, Title = "" }; ;
            StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                var resultado = await _webShopRepository.ConsultaPersonaPorDNIoRUC(entity);
                if (resultado == null)
                {
                    formulario = await _maestroPersonaRepository.SaveLAyherShop(entity);
                    if (formulario.Success)
                    {
                        formulario = await this.ComplexResponse(() => _webShopRepository.ConsultaPersonaPorDNIoRUC(entity));

                        return formulario;
                    }
                }
                else
                {
                    entity.Persona = resultado.Persona;
                    respuesta = await _maestroPersonaRepository.Update(entity);
                    if (respuesta.Success)
                    {
                        formulario = await this.ComplexResponse(() => _webShopRepository.ConsultaPersonaPorDNIoRUC(entity));
                    }
                    else
                    {
                        formulario = await this.ComplexResponse(() => _webShopRepository.ConsultaPersonaPorDNIoRUC(entity));
                    }

                }


                return formulario;

            }
            catch (Exception ex)
            {
                throw;
            }
            return respuesta;
        }

        //public async Task<StatusResponse> generarXML(PedidoWebShop entity)
        //{
        //    StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };

        //    return respuesta;
        //}

        public async Task<StatusResponse> SavePreGuia(List<Producto> Productos, string documento, string json)
        {

            StatusReponse<Producto> productos = new StatusReponse<Producto>() { Success = false, Title = "" };
            StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            string PreGuiaNumero = "";
            try
            {
                if (Productos.Count < 1)
                {
                    respuesta.Detail = "NO Hay productos registrados";
                    respuesta.Success = false;
                    return respuesta;
                }
                using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var producto in Productos)
                    {
                        if (!producto.esServicio)
                        {
                            ControlCantidadesShop controlCantidades = new ControlCantidadesShop();
                            controlCantidades.vc_idCompany = "04000000";
                            controlCantidades.vc_idProducto = producto.idCodigoProducto;
                            controlCantidades.vc_Producto = producto.Descripcion;
                            controlCantidades.dbl_cantidad = producto.cantidad;
                            controlCantidades.dt_fechaAprobacion = DateTime.Now;
                            controlCantidades.dbl_CantidadRecibida = producto.cantidad;
                            controlCantidades.vc_idListaPrecio = producto.ListsPrecio;
                            controlCantidades.estado_pre = "AP";
                            controlCantidades.AlmacenCodigo = "ALM_MKT01";
                            controlCantidades.LiberaStockPreGuia = "1";
                            controlCantidades.rucCliente = documento;
                            controlCantidades.PreGuiaNumero = (PreGuiaNumero != "") ? PreGuiaNumero : null;
                            controlCantidades.jsonResult = json;
                            controlCantidades.nroPedido = producto.nroPedido;
                            controlCantidades.dlb_precio = producto.precioUnitario;
                            productos = await _ControlCantidadesPreguiaGuiaRepositorys.SaveControlcantidadesEcommerce(controlCantidades);
                            PreGuiaNumero = productos.Data.PreGuiaNumero;
                            producto.PreGuiaNumero = PreGuiaNumero;
                            if (!productos.Success)
                            {
                                respuesta.Detail = "Error grabar Productos.....";
                                respuesta.Success = false;
                                return respuesta;
                            }
                        }

                    }
                    if (productos.Success)
                    {
                        respuesta.Detail = "Productos Registrados.";
                        respuesta.Success = true;
                        tran1.Complete();
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return respuesta;
        }
        public async Task<StatusResponse> SaveFacturacion(PedidoWebShop entity)
        {
            StatusReponse<VentaCabecera> ventaCabecera = new StatusReponse<VentaCabecera>() { Success = false, Title = "" };
            StatusReponse<VentaDetalle> ventaDetalle = new StatusReponse<VentaDetalle>() { Success = false, Title = "" };
            StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (entity.Productos.Count < 1)
                {
                    respuesta.Detail = "NO Hay productos para registrados";
                    respuesta.Success = false;
                    return respuesta;
                }
                MaestroPersonas validarPersonaExistente = await _webShopRepository.ConsultaPersonaPorDNIoRUC(entity.persona);
                if (validarPersonaExistente.Persona == 0)
                {
                    respuesta.Detail = "NO existe Usuario en nuestra base de datos";
                    respuesta.Success = false;
                    return respuesta;
                }
                else
                {
                    entity.persona = validarPersonaExistente;
                    VentaCabecera _ventaCabecera = CrearModeloVenta(entity);
                    using (var tran2 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        ventaCabecera = await _FacturacionRepository.SaveVentaCabecera(_ventaCabecera);
                        if (ventaCabecera.Success)
                        {

                            foreach (var _ventaDetalle in _ventaCabecera.VentaDetalles)
                            {
                                _ventaDetalle.NumeroDocumento = ventaCabecera.Data.NumeroDocumento;
                                ventaDetalle = await _FacturacionRepository.SaveVentaDetalle(_ventaDetalle);
                                if (!ventaDetalle.Success)
                                {
                                    respuesta.Detail = "Error grabar Productos en detalle.....";
                                    respuesta.Success = false;
                                    return respuesta;
                                }
                            }

                        }
                        else
                        {
                            respuesta.Detail = "Error al grabar Venta Cabecera...";
                            respuesta.Success = false;
                            return respuesta;
                        }
                        if (ventaCabecera.Success && ventaDetalle.Success)
                        {
                            tran2.Complete();
                            respuesta.Detail = "Se registro ok la solicitud...";
                            respuesta.Title = "OK";
                            respuesta.Success = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return respuesta;
        }

        private VentaCabecera CrearModeloVenta(PedidoWebShop entity)
        {
            VentaCabecera _ventaCabecera = new VentaCabecera();
            string preGuia = entity.Productos.FirstOrDefault().nroPedido;

            List<VentaDetalle> listDetalle = new List<VentaDetalle>();
            _ventaCabecera.CompaniaSocio = "04000000";
            _ventaCabecera.TipoDocumento = "PE";//solicitud de venta
            _ventaCabecera.NumeroDocumento = null;
            _ventaCabecera.EstablecimientoCodigo = "0006";
            _ventaCabecera.FormaFacturacion = "FG";
            _ventaCabecera.ClienteNumero = entity.persona.Persona;
            _ventaCabecera.ClienteRUC = entity.persona.Documento;
            _ventaCabecera.ClienteNombre = entity.persona.NombreCompleto;
            _ventaCabecera.ClienteDireccion = entity.persona.Direccion;
            _ventaCabecera.ClienteCobrarA = entity.persona.Persona;
            _ventaCabecera.FechaDocumento = DateTime.Now;
            _ventaCabecera.FechaVencimiento = DateTime.Now;
            _ventaCabecera.TipoFacturacion = "NO";
            _ventaCabecera.TipoVenta = "NOR";
            _ventaCabecera.ConceptoFacturacion = "SER";
            _ventaCabecera.Vendedor = "6003941";
            _ventaCabecera.FormadePago = "099";
            _ventaCabecera.TipodeCambio = 0.0;
            _ventaCabecera.MonedaDocumento = entity.MonedaDocumento;
            _ventaCabecera.MontoAfecto = double.Parse(((entity.montoAfecto / 1.18)).ToString());//Monto total
            _ventaCabecera.MontoNoAfecto = entity.montoNoAfecto; //0
            _ventaCabecera.MontoDescuentos = (entity.montoDescuento == 0) ? 0 : double.Parse((entity.montoDescuento / 1.18).ToString());
            _ventaCabecera.MontoTotal = (_ventaCabecera.MontoDescuentos > 0) ? (_ventaCabecera.MontoAfecto - _ventaCabecera.MontoDescuentos) : _ventaCabecera.MontoAfecto;  // subtal 
            //_ventaCabecera.MontoImpuestos = double.Parse((_ventaCabecera.MontoTotal * 0.18).ToString()); // igv
            _ventaCabecera.MontoImpuestoVentas = double.Parse((_ventaCabecera.MontoTotal * 0.18).ToString());
            _ventaCabecera.PreparadoPor = 6003941;
            _ventaCabecera.ComercialPedidoNumero = "0";
            _ventaCabecera.AlmacenCodigo = "ALM_MKT01";
            _ventaCabecera.CentroCosto = "090902";
            _ventaCabecera.Proyecto = "0";
            _ventaCabecera.UnidadNegocio = "0001";
            _ventaCabecera.UnidadReplicacion = "0001";
            _ventaCabecera.Estado = "PR";
            _ventaCabecera.Comentarios = "Compra LayherShop";
            _ventaCabecera.UltimaFechaModif = DateTime.Now;
            _ventaCabecera.TipoFormadePago = "6";
            _ventaCabecera.TipoListaPrecio = "12";
            _ventaCabecera.vc_TipoSolVta = "VTA";
            _ventaCabecera.MontoDetraccion = 0.0;
            _ventaCabecera.PorcentajeDetraccion = "0";
            _ventaCabecera.PorcentajeDetraccion = "0";
            _ventaCabecera.Monto_NC = 0.00;
            _ventaCabecera.nroGuiaShop = preGuia;
            entity.Productos.ForEach(detalle =>
            {
                VentaDetalle ventaDetalle = new VentaDetalle();
                ventaDetalle.TipoDocumento = "PE"; //solicitud de venta
                ventaDetalle.Linea = int.Parse(detalle.Secuencia);
                ventaDetalle.CompaniaSocio = "04000000";
                ventaDetalle.TipoDetalle = "S";
                ventaDetalle.ItemCodigo = detalle.itemCodigo;
                ventaDetalle.Descripcion = detalle.Descripcion;
                ventaDetalle.CantidadPedida = detalle.cantidad;
                ventaDetalle.CantidadEntregada = 0;
                ventaDetalle.CantidadEntregadaDoble = 0;
                ventaDetalle.PrecioUnitario = double.Parse(((detalle.precioUnitario / 1.18)).ToString());//detalle.precioUnitario; /*con igv*/
                ventaDetalle.PrecioUnitarioFinal = double.Parse(((detalle.precioUnitario / 1.18)).ToString());
                ventaDetalle.Monto = (detalle.precioUnitario);
                ventaDetalle.MontoFinal = (double.Parse(((detalle.precioUnitario / 1.18)).ToString()) * detalle.cantidad);
               // ventaDetalle.PrecioUnitarioFinal = detalle.precioUnitario;
                ventaDetalle.IGVExoneradoFlag = "N";
                ventaDetalle.AlmacenCodigo = "ALM_MKT01";
                ventaDetalle.Estado = "PR";
                ventaDetalle.UltimoUsuario = "ADMIN";
                ventaDetalle.PrecioUnitarioDoble = 0.0;
                ventaDetalle.PrecioUnitarioOriginal = 0.0;
                ventaDetalle.PrecioUnitarioGratuito = 0.0;
                ventaDetalle.PorcentajeDescuento01 = 0.0;
                ventaDetalle.PorcentajeDescuento02 = 0.0;
                ventaDetalle.PorcentajeDescuento03 = 0.0;
                ventaDetalle.CentroCosto = "090902";
                ventaDetalle.MontoDescuento01 = 0.0;
                ventaDetalle.MontoDescuento02 = 0.0;
                ventaDetalle.MontoDescuento03 = 0.0;
                ventaDetalle.UltimaFechaModif = DateTime.Now;
                listDetalle.Add(ventaDetalle);
            });
            _ventaCabecera.VentaDetalles = listDetalle;



            return _ventaCabecera;
        }
    }
}
