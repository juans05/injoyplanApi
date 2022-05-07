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
    public class TipoImportacionApp : BaseApp<TipoImportacionApp>
    {
        private ITipoImportacionRepository _tipoImportacionRepository;
        public TipoImportacionApp(ITipoImportacionRepository tipoImportacionRepository,
                                  ILogger<BaseApp<TipoImportacionApp>> logger) : base()
        {
            this._tipoImportacionRepository = tipoImportacionRepository;
        }
        public async Task<StatusReponse<List<TipoImportacion>>> List(int id)
        {
            return await this.ComplexResponse(() => _tipoImportacionRepository.List(id));
        }
    }
}
