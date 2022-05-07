using Dapper;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using LAYHER.Backend.Domain.ModuloPreguia_Guia.Interfaces;
using LAYHER.Backend.Domain.ModuloProductos.DTO;
using LAYHER.Backend.Domain.ModuloProductos.Interfaces;
using LAYHER.Backend.Domain.ModuloWebShop.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloPreguia_Guia
{
    public class PreGuia_guiaRepository : IControlCantidadesPreguiaGuiaRepository, IProductoRepository
    {
        protected readonly ICustomConnection mConnection;

        public PreGuia_guiaRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }
        public async Task<StatusReponse<Producto>> SaveControlcantidadesEcommerce(ControlCantidadesShop controlCantidades)
        {
            StatusReponse<Producto> status = new StatusReponse<Producto>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Producto>("[ALMACEN].[USP_INSERTAR_CONTROL_CANTIDAD_DETALLE_ECOMMERCE]",
                    new
                    {
                        @CompaniaSocio = controlCantidades.vc_idCompany,
                        @vc_idProducto = controlCantidades.vc_idProducto,
                        @vc_condicion = "0",
                        @vc_tipo = "NS",
                        @dbl_cantidad = controlCantidades.dbl_cantidad,
                        @ultimoUsuario = "ADMIN",
                        @UltimaFechaModif = DateTime.Now,
                        @almacenCodigo = controlCantidades.AlmacenCodigo,
                        @PreGuiaNumero = controlCantidades.PreGuiaNumero,
                        @rucCliente = controlCantidades.rucCliente,
                        @jsonResult = controlCantidades.jsonResult,
                        @NroPedido = controlCantidades.nroPedido,
                        @precio = controlCantidades.dlb_precio
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Producto)items.FirstOrDefault();

                    status.Success = true;
                }
                catch (Exception e)
                {
                    status.Success = false;
                }
            }
            return status;
        }
        //public async Task<List<Eventos>> ListaEventos()
        //{
        //    List<Eventos> entity = new List<Eventos>();

        //    using (var scope = await mConnection.BeginConnection())
        //    {
        //        try
        //        {
        //            var items = await scope.QueryAsync<Eventos>("[ALMACEN].[USP_CONSULTAR_PRODUCTO_ALMACEN_SHOP]", null, commandType: CommandType.StoredProcedure);
        //            foreach (var item in items)
        //            {
        //                Eventos producto = new Eventos();
        //                producto.dia = item.itemCodigo;
        //                producto.diaNum = item.idCodigoProducto;
        //                producto.mes = 1;
        //                entity.Add(producto);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new CustomException("Sucedió un error al realizar la operación");
        //        }
        //    }
        //    return entity;
        //}

        public async Task<List<Producto>> ListarProductos()
        {
            List<Producto> entity = new List<Producto>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Producto>("[ALMACEN].[USP_CONSULTAR_PRODUCTO_ALMACEN_SHOP]", null, commandType: CommandType.StoredProcedure);
                    foreach (var item in items)
                    {
                        Producto producto = new Producto();
                        producto.itemCodigo = item.itemCodigo;
                        producto.idCodigoProducto = item.idCodigoProducto;
                        producto.vc_rutaImagenLarga = (item.nombreImagen == null) ? "" : String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes("D:\\Layherp\\Almacen\\GestionInventarios" + "\\" + item.nombreImagen)));
                        producto.nombreImagen = item.nombreImagen;
                        producto.Descripcion = item.Descripcion;
                        producto.precioUnitario = item.precioUnitario;
                        producto.stockActual = item.stockActual;
                        producto.condicion = item.condicion;
                        producto.ListsPrecio = item.ListsPrecio;
                        entity.Add(producto);

                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
    }
}
