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
    public class PerfilApp : BaseApp<PerfilApp>
    {
        private IPerfilRepository _perfilRepository;
        public PerfilApp(IPerfilRepository perfilRepository,
                         ILogger<BaseApp<PerfilApp>> logger) : base()
        {
            this._perfilRepository = perfilRepository;
        }
        public async Task<StatusReponse<List<Perfil>>> List(int id)
        {
            return await this.ComplexResponse(() => _perfilRepository.List(id));
        }
        public async Task<StatusReponse<List<Perfil>>> ListByDocumento(string documento)
        {
            StatusReponse<List<Perfil>> status = null;
            if (string.IsNullOrEmpty(documento))
            {
                status = new StatusReponse<List<Perfil>>(false, "Debe especificar el nro. de documento del usuario.");
                return status;
            }
            status = await this.ComplexResponse(() => _perfilRepository.ListByDocumento(documento));
            return status;
        }
        public async Task<StatusReponse<List<PerfilPersona>>> ListPerfilPersona(int id)
        {
            return await this.ComplexResponse(() => _perfilRepository.ListPerfilPersona(id));
        }
        public async Task<StatusReponse<List<PerfilPermiso>>> ListPerfilPermiso(int perfil, int permiso)
        {
            return await this.ComplexResponse(() => _perfilRepository.ListPerfilPermiso(perfil, permiso));
        }
        public async Task<StatusReponse<PerfilPersona>> SavePerfilPersona(PerfilPersona entity)
        {
            StatusReponse<PerfilPersona> response = new StatusReponse<PerfilPersona>();

            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _perfilRepository.SavePerfilPersona(entity);
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
