using Dapper;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloProgramacionUnidad
{
    public class TipoImportacionRepository : ITipoImportacionRepository
    {
        protected readonly ICustomConnection mConnection;

        public TipoImportacionRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<TipoImportacion>> List(int id)
        {
            List<TipoImportacion> response = new List<TipoImportacion>();

            List<TipoImportacion> entity = new List<TipoImportacion>
            {
                new TipoImportacion { IdTipoImportacion = 1, Nombre = "Aérea" },
                new TipoImportacion { IdTipoImportacion = 2, Nombre = "Marítima" },
                new TipoImportacion { IdTipoImportacion = 3, Nombre = "Terrestre" }
            };

            try
            {
                response = entity.Where(x => x.IdTipoImportacion == id || 0 == id).ToList();
            }
            catch (Exception)
            {
                throw new CustomException("Sucedió un error al realizar la operación");
            }

            return response;
        }
    }
}
