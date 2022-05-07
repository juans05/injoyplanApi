using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente
{
    public class PermisoApp : BaseApp<PermisoApp>
    {
        private IPermisoRepository _permisoRepository;
        public PermisoApp(IPermisoRepository permisoRepository,
                          ILogger<BaseApp<PermisoApp>> logger) : base()
        {
            this._permisoRepository = permisoRepository;
        }
        public async Task<StatusReponse<List<Permiso>>> List(int id)
        {
            return await this.ComplexResponse(() => _permisoRepository.List(id));
        }
        public async Task<StatusReponse<List<PermisoPerfilPersona>>> ListPermisoPerfilPersona(int id)
        {
            return await this.ComplexResponse(() => _permisoRepository.ListPermisoPerfilPersona(id));
        }
        public async Task<StatusReponse<List<PermisoPerfilPersona>>> ListPermisoProyectoPerfilPersona(int persona, int perfil, string afe, string documento, string nombre)
        {
            StatusReponse<List<PermisoPerfilPersona>> status = null;
            if (string.IsNullOrEmpty(afe))
            {
                status = new StatusReponse<List<PermisoPerfilPersona>>(false, "Debe especificar el Proyecto.");
                return status;
            }
            if (string.IsNullOrEmpty(documento))
            {
                status = new StatusReponse<List<PermisoPerfilPersona>>(false, "Debe especificar el nro. de documento del usuario.");
                return status;
            }
            status = await this.ComplexResponse(() => _permisoRepository.ListPermisoProyectoPerfilPersona(persona,perfil,afe,documento,nombre));
            return status;
        }
        public async Task<StatusResponse> SavePermisoPerfilPersona(PermisoPerfilPersona entity)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _permisoRepository.SavePermisoPerfilPersona(entity);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
