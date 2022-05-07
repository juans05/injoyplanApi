using Dapper;
using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Domain.Comun.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.Comun
{
    public class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        protected readonly ICustomConnection mConnection;

        public TipoDocumentoRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<TipoDocumento>> List(int id, int activo)
        {
            List<TipoDocumento> entity = new List<TipoDocumento>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<TipoDocumento>("[AUC].[Usp_ListaTipoDocumento]",
                    new
                    {
                        @Id = id,
                        @Estado = activo
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<TipoDocumento>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
    }
}
