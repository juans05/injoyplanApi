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
    public class CumplimientoRepository : ICumplimientoRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<CumplimientoRepository> _logger;

        public CumplimientoRepository(ICustomConnection _connection, ILogger<CumplimientoRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }
        public async Task<StatusReponse<Cumplimiento>> Registrar(Cumplimiento cumplimiento)
        {
            StatusReponse<Cumplimiento> status = new StatusReponse<Cumplimiento>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Cumplimiento>("[INSPECCION].[Usp_CRUD_Cumplimiento]",
                    new
                    {
                        @strMode = "C",
                        @CheckList_id = cumplimiento.CheckList_id,
                        @PreguntaInspeccionAndamio_id = cumplimiento.PreguntaInspeccionAndamio_id,
                        @RespuestaCumplimiento_id = cumplimiento.RespuestaCumplimiento_id,
                        @IdUsuarioCreacion = cumplimiento.IdUsuarioCreacion,
                        @IdUsuarioEdicion = cumplimiento.IdUsuarioEdicion,
                        @FechaCreacion = cumplimiento.FechaCreacion,
                        @FechaEdicion = cumplimiento.FechaEdicion,
                        @Activo = cumplimiento.Activo
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Cumplimiento)items.FirstOrDefault();
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

        public async Task<StatusResponse> Eliminar(int CheckList_id)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[INSPECCION].[Usp_CRUD_Cumplimiento]",
                    new
                    {
                        @strMode = "D",
                        @CheckList_id = CheckList_id
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