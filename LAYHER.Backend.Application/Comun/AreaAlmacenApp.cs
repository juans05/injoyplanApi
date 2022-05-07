using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Domain.Comun.Interfaces;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.Comun
{
    public class AreaAlmacenApp : BaseApp<AreaAlmacenApp>
    {
        private IAreaAlmacenRepository _areaAlmacenRepository;
        public AreaAlmacenApp(IAreaAlmacenRepository areaAlmacenRepository) : base()
        {
            this._areaAlmacenRepository = areaAlmacenRepository;
        }
        public async Task<StatusReponse<List<RegionAlmacen>>> ListSede(int id)
        {
            return await this.ComplexResponse(() => _areaAlmacenRepository.ListSede(id));
        }
        public async Task<StatusReponse<List<AreaAlmacen>>> ListAlmacen(int id)
        {
            return await this.ComplexResponse(() => _areaAlmacenRepository.ListAlmacen(id));
        }
        public async Task<StatusReponse<List<AreaAlmacen>>> ListTodosAlmacenes()
        {
            return await this.ComplexResponse(() => _areaAlmacenRepository.ListTodosAlmacenes());
        }

        public async Task<StatusReponse<Almacen>> ObtenerAlmacenPorId(string idAlmacen)
        {
            return await this.ComplexResponse(() => _areaAlmacenRepository.ObtenerAlmacenPorId(idAlmacen));
        }
    }
}
