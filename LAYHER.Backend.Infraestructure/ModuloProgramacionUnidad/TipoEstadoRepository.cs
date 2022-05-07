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
    public class TipoEstadoRepository : ITipoEstadoRepository
    {
        protected readonly ICustomConnection mConnection;

        public TipoEstadoRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<TipoEstado>> List(int id)
        {
            List<TipoEstado> entity = new List<TipoEstado>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<TipoEstado>("[PU].[Usp_ListaTipoEstado]",
                    new
                    {
                        @Id = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<TipoEstado>)items;
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
