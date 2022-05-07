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
    public class ConsideracionRepository : IConsideracionRepository
    {
        protected readonly ICustomConnection mConnection;

        public ConsideracionRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Consideracion>> List(int id)
        {
            List<Consideracion> entity = new List<Consideracion>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Consideracion>("[PU].[Usp_ListaDocumentosxConsiderar]",
                    new
                    {
                        @Consideracion = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Consideracion>)items;
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
