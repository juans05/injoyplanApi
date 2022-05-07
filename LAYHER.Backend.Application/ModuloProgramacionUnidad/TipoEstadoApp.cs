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
    public class TipoEstadoApp : BaseApp<TipoEstadoApp>
    {
        private ITipoEstadoRepository _tipoEstadoRepository;
        public TipoEstadoApp(ITipoEstadoRepository tipoEstadoRepository,
                             ILogger<BaseApp<TipoEstadoApp>> logger) : base()
        {
            this._tipoEstadoRepository = tipoEstadoRepository;
        }
        public async Task<StatusReponse<List<TipoEstado>>> List(int id)
        {
            return await this.ComplexResponse(() => _tipoEstadoRepository.List(id));
        }
    }
}
