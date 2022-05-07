using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces
{
    public interface ITipoProgramacionRepository
    {
        Task<List<TipoProgramacion>> List(int id);

        Task<List<ProgramacionEstado>> ListProgramacionEstado(int id);
    }
}
