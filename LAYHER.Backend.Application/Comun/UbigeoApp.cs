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
    public class UbigeoApp : BaseApp<UbigeoApp>
    {
        private IZonaRepository _zonaRepository;
        public UbigeoApp(IZonaRepository zonaRepository) : base()
        {
            this._zonaRepository = zonaRepository;
        }
        public async Task<StatusReponse<List<Zona>>> List(int id)
        {
            return await this.ComplexResponse(() => _zonaRepository.List(id));
        }
        public async Task<StatusReponse<List<Zona>>> ListDepartamento()
        {
            return await this.ComplexResponse(() => _zonaRepository.ListDepartamento());
        }
        public async Task<StatusReponse<List<Zona>>> ListProvincia(string departamento)
        {
            return await this.ComplexResponse(() => _zonaRepository.ListProvincia(departamento));
        }
        public async Task<StatusReponse<List<Zona>>> ListDistrito(string provincia)
        {
            return await this.ComplexResponse(() => _zonaRepository.ListDistrito(provincia));
        }
    }
}
