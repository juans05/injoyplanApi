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
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;

namespace LAYHER.Backend.Infraestructure.ModuloInspeccionAndamios
{
    public class CheckListRepository : ICheckListRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<CheckListRepository> _logger;

        public CheckListRepository(ICustomConnection _connection, ILogger<CheckListRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }
        public async Task<StatusReponse<CheckList>> Registrar(CheckList checkList)
        {
            StatusReponse<CheckList> status = new StatusReponse<CheckList>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<CheckList>("[INSPECCION].[Usp_CRUD_CheckList]",
                    new
                    {
                        @strMode = checkList.strMode,
                        @CheckList_id = checkList.CheckList_id,
                        @InspeccionAndamio_id = checkList.InspeccionAndamio_id,
                        @TipoAndamio_id = checkList.TipoAndamio_id,
                        @SubTipoAndamio_id = checkList.SubTipoAndamio_id,
                        @IdUsuarioCreacion = checkList.IdUsuarioCreacion,
                        @IdUsuarioEdicion = checkList.IdUsuarioEdicion,
                        @FechaCreacion = checkList.FechaCreacion,
                        @FechaEdicion = checkList.FechaEdicion,
                        @Activo = checkList.Activo
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (CheckList)items.FirstOrDefault();
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
        public async Task<OutCheckList> Obtener(int checkList_id, int? inspeccionAndamio_id, bool incluirCumplimiento = false)
        {
            OutCheckList item = null;
            DynamicParameters dynamicParameters = new DynamicParameters();
            using (var scope = await mConnection.BeginConnection())
            {
                item = await scope.QueryFirstOrDefaultAsync<OutCheckList>("[INSPECCION].[Usp_CRUD_CheckList]",
                new
                {
                    @strMode = "R",
                    @checkList_id = checkList_id,
                    @InspeccionAndamio_id = inspeccionAndamio_id
                }, commandType: CommandType.StoredProcedure);

                if (item == null)
                    throw new CustomException("El registro no existe");

                if (item.CheckList_id > 0 && incluirCumplimiento)
                {
                    dynamicParameters.Add("@strMode", "R", DbType.String);
                    dynamicParameters.Add("@CheckList_id", checkList_id, DbType.Int32);

                    var listaPregunta = await scope.QueryAsync<PreguntaInspeccion, Cumplimiento, PreguntaInspeccion>("[INSPECCION].[Usp_CRUD_Cumplimiento]", (p, c) =>
                       {
                           p.Cumplimiento = c;
                           return p;
                       }, dynamicParameters, splitOn: "Cumplimiento_id", commandType: CommandType.StoredProcedure);

                    item.ListaPreguntaInspeccion = listaPregunta.ToList();
                }

            }
            return item;
        }

        public async Task<StatusResponse> Eliminar(int CheckList_id)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[INSPECCION].[Usp_CRUD_CheckList]",
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