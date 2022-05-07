using Dapper;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloProgramacionUnidad
{
    public class TipoUnidadRepository : ITipoUnidadRepository
    {
        protected readonly ICustomConnection mConnection;

        public TipoUnidadRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<TipoUnidad>> List(int id)
        {
            List<TipoUnidad> entity = new List<TipoUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<TipoUnidad>("[PU].[Usp_ListaTipoUnidad]",
                    new
                    {
                        @Id = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<TipoUnidad>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<UnidadMetraje>> ListUnidadMetraje(int id)
        {
            List<UnidadMetraje> entity = new List<UnidadMetraje>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<UnidadMetraje>("[PU].[Usp_ListaTipoUnidadxMetraje]",
                    new
                    {
                        @Id = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<UnidadMetraje>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
    }
}
