using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.ModuloProgramacionUnidad
{
    public class EspecificacionApp : BaseApp<EspecificacionApp>
    {
        private IEspecificacionRepository _especificacionRepository;
        public EspecificacionApp(IEspecificacionRepository especificacionRepository,
                                 ILogger<BaseApp<EspecificacionApp>> logger) : base()
        {
            this._especificacionRepository = especificacionRepository;
        }
        public async Task<StatusReponse<List<Especificacion>>> List(int id)
        {
            return await this.ComplexResponse(() => _especificacionRepository.List(id));
        }
    }
}
