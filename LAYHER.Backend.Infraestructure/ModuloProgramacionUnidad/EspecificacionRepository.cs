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
    public class EspecificacionRepository : IEspecificacionRepository
    {
        protected readonly ICustomConnection mConnection;

        public EspecificacionRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Especificacion>> List(int id)
        {
            List<Especificacion> entities = new List<Especificacion>();

            List<Especificacion> entity = new List<Especificacion>
            {
                new Especificacion { IdEspecificacion = 1, Nombre = "Programado" },
                new Especificacion { IdEspecificacion = 2, Nombre = "No Programado" }
            };
            try
            {
                entities = entity.Where(x => x.IdEspecificacion == id || 0 == id).ToList();
            }
            catch (Exception)
            {
                throw new CustomException("Sucedió un error al realizar la operación");
            }

            return entities;
        }
    }
}
