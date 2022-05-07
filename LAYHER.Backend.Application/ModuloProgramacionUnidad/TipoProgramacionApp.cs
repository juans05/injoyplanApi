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
    public class TipoProgramacionApp : BaseApp<TipoProgramacionApp>
    {
        private ITipoProgramacionRepository _tipoProgramacionRepository;
        public TipoProgramacionApp(ITipoProgramacionRepository tipoProgramacionRepository,
                                   ILogger<BaseApp<TipoProgramacionApp>> logger) : base()
        {
            this._tipoProgramacionRepository = tipoProgramacionRepository;
        }
        public async Task<StatusReponse<List<TipoProgramacion>>> List(int id)
        {
            return await this.ComplexResponse(() => _tipoProgramacionRepository.List(id));
        }
        public async Task<StatusReponse<List<ProgramacionEstado>>> ListProgramacionEstado(int id)
        {
            return await this.ComplexResponse(() => _tipoProgramacionRepository.ListProgramacionEstado(id));
        }
    }
}
