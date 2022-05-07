using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente
{
    public class UsuarioApp : BaseApp<UsuarioApp>
    {
        private IUsuarioRepository _usuarioRepository;
        public UsuarioApp(IUsuarioRepository usuarioRepository) 
        {
            this._usuarioRepository = usuarioRepository;
        }

        public async Task<StatusReponse<Usuario>> ObtenerPerfilPorNroDocumento(string nroDocumento)
        {
            StatusReponse<Usuario> status;
            if (string.IsNullOrEmpty(nroDocumento))
            {
                status = new StatusReponse<Usuario>(false, "Ingrese un número de documento válido");
                return status;
            }

            status = await this.ComplexResponse(() => _usuarioRepository.ObtenerUsuarioPorNroDocumento(nroDocumento), "");
            return status;
        }
    }
}
