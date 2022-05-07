using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente
{
    public class MaestroProyectosApp : BaseApp<MaestroProyectosApp>
    {
        private IMaestroProyectosRepository _maestroProyectosRepository;
        public MaestroProyectosApp(IMaestroProyectosRepository maestroProyectosRepository,
                                   ILogger<BaseApp<MaestroProyectosApp>> logger) : base()
        {
            this._maestroProyectosRepository = maestroProyectosRepository;
        }
        public async Task<StatusReponse<List<MaestroProyectos>>> List(string id, int offset, int fetch)
        {
            return await this.ComplexResponse(() => _maestroProyectosRepository.List(id,offset,fetch));
        }
        public async Task<StatusReponse<List<MaestroProyectos>>> ListProyectoCliente(string id)
        {
            StatusReponse<List<MaestroProyectos>> status = null;
            if (string.IsNullOrEmpty(id))
            {
                status = new StatusReponse<List<MaestroProyectos>>(false, "Debe especificar el Proyecto.");
                return status;
            }
            status = await this.ComplexResponse(() => _maestroProyectosRepository.ListProyectoCliente(id));
            return status;
        }
    }
}
