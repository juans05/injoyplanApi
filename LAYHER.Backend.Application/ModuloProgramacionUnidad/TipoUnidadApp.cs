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
    public class TipoUnidadApp : BaseApp<TipoUnidadApp>
    {
        private ITipoUnidadRepository _tipoUnidadRepository;
        public TipoUnidadApp(ITipoUnidadRepository tipoUnidadRepository,
                             ILogger<BaseApp<TipoUnidadApp>> logger) : base()
        {
            this._tipoUnidadRepository = tipoUnidadRepository;
        }
        public async Task<StatusReponse<List<TipoUnidad>>> List(int id)
        {
            return await this.ComplexResponse(() => _tipoUnidadRepository.List(id));
        }

        public async Task<StatusReponse<List<UnidadMetraje>>> ListUnidadMetraje(int id)
        {
            return await this.ComplexResponse(() => _tipoUnidadRepository.ListUnidadMetraje(id));
        }
    }
}
