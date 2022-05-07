using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Domain.Comun.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.Comun
{
    public class TipoDocumentoApp : BaseApp<TipoDocumentoApp>
    {
        private ITipoDocumentoRepository _tipoDocumentoRepository;
        public TipoDocumentoApp(ITipoDocumentoRepository tipoDocumentoRepository
                                ) : base()
        {
            this._tipoDocumentoRepository = tipoDocumentoRepository;
        }
        public async Task<StatusReponse<List<TipoDocumento>>> List(int id, int activo)
        {
            return await this.ComplexResponse(() => _tipoDocumentoRepository.List(id, activo));
        }
    }
}
