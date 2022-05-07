using Dapper;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloWebShop.DTO;
using LAYHER.Backend.Domain.ModuloWebShop.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloWebShop
{
    public class WebShopRepository : IWebShopRepository
    {
        protected readonly ICustomConnection mConnection;
        public WebShopRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }


        public Task<List<PedidoWebShop>> SavePreGuia(Formulario entity)
        {
            throw new NotImplementedException();
        }


        public Task<List<PedidoWebShop>> SaveVentaDetalle(Formulario entity)
        {
            throw new NotImplementedException();
        }
        public async Task<MaestroPersonas> ConsultaPersonaPorDNIoRUC(MaestroPersonas persona)
        {
            MaestroPersonas entity = new MaestroPersonas();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[COMUN].[USP_VALIDARPERSONAXTIPODOCUMENTO]",
                    new
                    {
                        @Documento = persona.Documento,
                        @TipoDocumento = persona.TipoDocumento
                    }, commandType: CommandType.StoredProcedure);
                    entity = (MaestroPersonas)items.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<MaestroPersonas> ConsultaPersonaPorDNIoRUC(string tipoDocumentoPersonaEmpresa, string nroDocumentoPersonaEmpresa)
        {
            MaestroPersonas entity = new MaestroPersonas();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[COMUN].[USP_VALIDARPERSONAXTIPODOCUMENTO]",
                    new
                    {
                        @Documento = nroDocumentoPersonaEmpresa,
                        @TipoDocumento = tipoDocumentoPersonaEmpresa
                    }, commandType: CommandType.StoredProcedure);
                    entity = (MaestroPersonas)items.FirstOrDefault();
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
