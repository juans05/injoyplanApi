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
    public class MetrajeRepository : IMetrajeRepository
    {
        protected readonly ICustomConnection mConnection;

        public MetrajeRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Metraje>> List(int id)
        {
            List<Metraje> entity = new List<Metraje>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Metraje>("[PU].[Usp_ListaMetrajes]",
                    new
                    {
                        @Id = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Metraje>)items;
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
