using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Shared;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;

namespace LAYHER.Backend.Domain.Inspeccion.Interfaces
{
    public interface IConfiguracionAppRepository
    {
        Task<StatusResponse> CreateUpdate(ConfiguracionApp configuracion);
        Task<OutConfiguracionApp> Obtener(int usuario_id);
    }
}