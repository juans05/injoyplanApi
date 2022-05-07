using Dapper;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using LAYHER.Backend.Domain.Inspeccion.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Shared;

namespace LAYHER.Backend.Infraestructure.ModuloInspeccionAndamios
{
    public class RegistroFotograficoRepository : IRegistroFotograficoRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<RegistroFotograficoRepository> _logger;

        public RegistroFotograficoRepository(ICustomConnection _connection, ILogger<RegistroFotograficoRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }
        public async Task<StatusReponse<RegistroFotografico>> Registrar(RegistroFotografico registroFotografico)
        {
            StatusReponse<RegistroFotografico> status = new StatusReponse<RegistroFotografico>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<RegistroFotografico>("[INSPECCION].[Usp_CRUD_RegistroFotografico]",
                    new
                    {
                        @strMode = registroFotografico.strMode,
                        @RegistroFotografico_id = registroFotografico.RegistroFotografico_id,
                        @InspeccionAndamio_id = registroFotografico.InspeccionAndamio_id,
                        @Descripcion = registroFotografico.Descripcion,
                        @Nombre = registroFotografico.Nombre,
                        @NombreOriginal = registroFotografico.NombreOriginal,
                        @EsFoto = registroFotografico.EsFoto,
                        @IdUsuarioCreacion = registroFotografico.IdUsuarioCreacion,
                        @IdUsuarioEdicion = registroFotografico.IdUsuarioEdicion,
                        @FechaCreacion = registroFotografico.FechaCreacion,
                        @FechaEdicion = registroFotografico.FechaEdicion,
                        @Activo = registroFotografico.Activo
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (RegistroFotografico)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception ex)
                {
                    throw;
                    status.Detail = ex.Message;
                }
            }
            return status;
        }
        public async Task<List<RegistroFotografico>> Listar(int inspeccionAndamio_id)
        {
            List<RegistroFotografico> lista = new List<RegistroFotografico>();
            DynamicParameters dynamicParameters = new DynamicParameters();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<RegistroFotografico>("[INSPECCION].[Usp_CRUD_RegistroFotografico]",
                    new
                    {
                        @strMode = "R1",
                        @InspeccionAndamio_id = inspeccionAndamio_id
                    }, commandType: CommandType.StoredProcedure);
                    lista = ((List<RegistroFotografico>)items);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return lista;
        }

        public async Task<RegistroFotografico> Obtener(string nombre)
        {
            RegistroFotografico item = null;
            DynamicParameters dynamicParameters = new DynamicParameters();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    item = await scope.QueryFirstOrDefaultAsync<RegistroFotografico>("[INSPECCION].[Usp_CRUD_RegistroFotografico]",
                    new
                    {
                        @strMode = "R2",
                        @Nombre = nombre
                    }, commandType: CommandType.StoredProcedure);
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return item;
        }

        public async Task<StatusResponse> Eliminar(int RegistroFotografico_id)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[INSPECCION].[Usp_CRUD_RegistroFotografico]",
                    new
                    {
                        @strMode = "D",
                        @RegistroFotografico_id = RegistroFotografico_id
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                }
                catch (Exception ex)
                {
                    throw;
                    status.Detail = ex.Message;
                }
            }
            return status;
        }
    }
}