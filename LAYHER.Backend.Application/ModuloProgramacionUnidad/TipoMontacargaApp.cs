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
    public class TipoMontacargaApp : BaseApp<TipoMontacargaApp>
    {
        private ITipoMontacargaRepository _tipoMontacargaRepository;
        public TipoMontacargaApp(ITipoMontacargaRepository tipoMontacargaRepository,
                                 ILogger<BaseApp<TipoMontacargaApp>> logger) : base()
        {
            this._tipoMontacargaRepository = tipoMontacargaRepository;
        }
        public async Task<StatusReponse<List<TipoMontacarga>>> List(int programacion, int id)
        {
            return await this.ComplexResponse(() => _tipoMontacargaRepository.List(programacion, id));
        }
    }
}
