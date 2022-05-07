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
    public class CotizacionApp : BaseApp<CotizacionApp>
    {
        private ICotizacionRepository _cotizacionRepository;
        public CotizacionApp(ICotizacionRepository cotizacionRepository,
                             ILogger<BaseApp<CotizacionApp>> logger) : base()
        {
            this._cotizacionRepository = cotizacionRepository;
        }
        public async Task<StatusReponse<List<Cotizacion>>> List(string afe)
        {
            StatusReponse<List<Cotizacion>> status = null;
            if (string.IsNullOrEmpty(afe))
            {
                status = new StatusReponse<List<Cotizacion>>(false, "Debe especificar el nro. cotizacion.");
                return status;
            }
            status = await this.ComplexResponse(() => _cotizacionRepository.List(afe));
            return status;
        }
    }
}
