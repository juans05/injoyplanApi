using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces
{
    public interface IProgramacionTiempoRepository
    {
        Task<List<ProgramacionTiempo>> List(int ptiempo, int punidad);
        Task<StatusReponse<ProgramacionTiempo>> Save(ProgramacionTiempo entity);
    }
}
