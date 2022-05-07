using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Shared;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;

namespace LAYHER.Backend.Domain.Inspeccion.Interfaces
{
    public interface ICheckListRepository
    {
        Task<StatusReponse<CheckList>> Registrar(CheckList checkList);
        Task<OutCheckList> Obtener(int checkList_id, int? inspeccionAndamio_id, bool incluirCumplimiento = false);
        Task<StatusResponse> Eliminar(int CheckList_id);
    }
}