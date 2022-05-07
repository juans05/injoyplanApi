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
    public class CotizacionRepository : ICotizacionRepository
    {
        protected readonly ICustomConnection mConnection;

        public CotizacionRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Cotizacion>> List(string afe)
        {
            List<Cotizacion> entity = new List<Cotizacion>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Cotizacion>("[PU].[Usp_ListaCotizacion]",
                    new
                    {
                        @Proyecto = afe
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Cotizacion>)items;
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
