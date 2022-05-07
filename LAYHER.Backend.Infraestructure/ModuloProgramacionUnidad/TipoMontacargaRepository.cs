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
    public class TipoMontacargaRepository : ITipoMontacargaRepository
    {
        protected readonly ICustomConnection mConnection;

        public TipoMontacargaRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<TipoMontacarga>> List(int programacion, int id)
        {
            List<TipoMontacarga> response = new List<TipoMontacarga>();
            try
            {
                if (programacion == 1)
                {
                    List<TipoMontacarga> entity = new List<TipoMontacarga>
                    {
                    new TipoMontacarga { IdTipoMontacarga = 1, Nombre = "Manual" },
                    new TipoMontacarga { IdTipoMontacarga = 2, Nombre = "3Tn" }
                    };

                    response = entity.Where(x => x.IdTipoMontacarga == id || 0 == id).ToList();
                }
                else
                {
                    List<TipoMontacarga> entity = new List<TipoMontacarga>
                    {
                    new TipoMontacarga { IdTipoMontacarga = 1, Nombre = "Manual" },
                    new TipoMontacarga { IdTipoMontacarga = 2, Nombre = "3Tn" },
                    new TipoMontacarga { IdTipoMontacarga = 3, Nombre = "10Tn" },
                    };

                    response = entity.Where(x => x.IdTipoMontacarga == id || 0 == id).ToList();
                }
            }
            catch (Exception)
            {
                throw new CustomException("Sucedió un error al realizar la operación");
            }
            return response;
        }
    }
}
