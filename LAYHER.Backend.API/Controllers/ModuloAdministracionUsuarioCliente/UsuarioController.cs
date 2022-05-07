using LAYHER.Backend.API.Helpers;
using LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Controllers.ModuloAdministracionUsuarioCliente
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
   
        private readonly AppSettings _appSettings;
        private readonly UsuarioApp _usuarioApp;

        public UsuarioController(UsuarioApp usuarioApp, IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
            this._usuarioApp = usuarioApp;
           
        }

        [HttpGet]
        [Route("perfil")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> ObtenerPerfil(string nroDocumento)
        {
            StatusReponse<Usuario> status = await _usuarioApp.ObtenerPerfilPorNroDocumento(nroDocumento);
            if (!status.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, status);
            return Ok(status.Data);
        }
    }
}
