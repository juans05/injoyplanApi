using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.Comun.Interfaces
{
    public interface IAreaAlmacenRepository
    {
        Task<List<AreaAlmacen>> ListAlmacen(int id);
        Task<List<AreaAlmacen>> ListTodosAlmacenes();
        Task<List<RegionAlmacen>> ListSede(int id);
        Task<Almacen> ObtenerAlmacenPorId(string idAlmacen);
    }
}
