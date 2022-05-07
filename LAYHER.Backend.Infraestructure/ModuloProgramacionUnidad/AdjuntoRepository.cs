using Dapper;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloProgramacionUnidad
{
    public class AdjuntoRepository : IAdjuntoRepository
    {
        protected readonly ICustomConnection mConnection;

        public AdjuntoRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Adjunto>> List(int id)
        {
            List<Adjunto> entity = new List<Adjunto>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Adjunto>("[PU].[Usp_ListaDocumentosxAdjuntar]",
                    new
                    {
                        @Adjunto = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Adjunto>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<ArchivoAdjunto> ListaAdjuntoFormulario(int id)
        {
            ArchivoAdjunto entity = new ArchivoAdjunto();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ArchivoAdjunto>("[PU].[Usp_ListaAdjuntoFormulario]",
                    new
                    {
                        @ArchivoAdjunto = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = items.FirstOrDefault();
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
