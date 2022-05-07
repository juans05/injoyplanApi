using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Shared;

namespace LAYHER.Backend.Domain.Inspeccion.Interfaces
{
    public interface ICumplimientoRepository
    {
        Task<StatusReponse<Cumplimiento>> Registrar(Cumplimiento cumplimiento);
        Task<StatusResponse> Eliminar (int CheckList_id);
    }
}