using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.ModuloProgramacionUnidad
{
    public class AdjuntoApp : BaseApp<AdjuntoApp>
    {
        private IAdjuntoRepository _adjuntoRepository;
        public AdjuntoApp(IAdjuntoRepository adjuntoRepository,
                          ILogger<BaseApp<AdjuntoApp>> logger) : base()
        {
            this._adjuntoRepository = adjuntoRepository;
        }
        public async Task<StatusReponse<List<Adjunto>>> List(int id)
        {
            return await this.ComplexResponse(() => _adjuntoRepository.List(id));
        }

        public async Task<StatusReponse<ArchivoAdjunto>> DescargaDocumentosAdjuntos(int id)
        {
            return await this.ComplexResponse(() => _adjuntoRepository.ListaAdjuntoFormulario(id));
        }
    }
}
