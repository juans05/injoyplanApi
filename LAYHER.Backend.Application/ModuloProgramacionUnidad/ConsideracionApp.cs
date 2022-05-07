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
    public class ConsideracionApp : BaseApp<ConsideracionApp>
    {
        private IConsideracionRepository _consideracionRepository;
        public ConsideracionApp(IConsideracionRepository consideracionRepository,
                                ILogger<BaseApp<ConsideracionApp>> logger) : base()
        {
            this._consideracionRepository = consideracionRepository;
        }
        public async Task<StatusReponse<List<Consideracion>>> List(int id)
        {
            return await this.ComplexResponse(() => _consideracionRepository.List(id));
        }
    }
}
