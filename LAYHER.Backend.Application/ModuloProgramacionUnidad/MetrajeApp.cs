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
    public class MetrajeApp : BaseApp<MetrajeApp>
    {
        private IMetrajeRepository _metrajeRepository;
        public MetrajeApp(IMetrajeRepository metrajeRepository,
                          ILogger<BaseApp<MetrajeApp>> logger) : base()
        {
            this._metrajeRepository = metrajeRepository;
        }
        public async Task<StatusReponse<List<Metraje>>> List(int id)
        {
            return await this.ComplexResponse(() => _metrajeRepository.List(id));
        }
    }
}
