using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces
{
    public interface ITipoUnidadRepository
    {
        Task<List<TipoUnidad>> List(int id);
        Task<List<UnidadMetraje>> ListUnidadMetraje(int id);
    }
}
