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
    public class ConfiguracionAppRepository : IConfiguracionAppRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<ConfiguracionAppRepository> _logger;

        public ConfiguracionAppRepository(ICustomConnection _connection, ILogger<ConfiguracionAppRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }
        public async Task<StatusResponse> CreateUpdate(ConfiguracionApp configuracion)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[INSPECCION].[Usp_CRUD_ConfiguracionApp]",
                    new
                    {
                        @strMode = "C",
                        @Usuario_id = configuracion.Usuario_id,
                        @ModoHistorico = configuracion.ModoHistorico,
                        @CantidadAnios = configuracion.CantidadAnios,
                        @IdUsuario = configuracion.IdUsuario,
                        @Fecha = configuracion.Fecha,
                        @Activo = configuracion.Activo,
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
        public async Task<OutConfiguracionApp> Obtener(int usuario_id)
        {
            OutConfiguracionApp item = null;
            DynamicParameters dynamicParameters = new DynamicParameters();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    item = await scope.QueryFirstOrDefaultAsync<OutConfiguracionApp>("[INSPECCION].[Usp_CRUD_ConfiguracionApp]",
                    new
                    {
                        @strMode = "R",
                        @Usuario_id = usuario_id
                    }, commandType: CommandType.StoredProcedure);
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return item;
        }
    }
}