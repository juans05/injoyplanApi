using LAYHER.Backend.API.Helpers;
using LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
//using LAYHER.Backend.Domain.Seguridad;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;

namespace LAYHER.Backend.API.Controllers.ModuloAdministracionUsuarioCliente
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {

        private readonly AppSettings _appSettings;
        private readonly SeguridadApp _seguridadApp;

        public SeguridadController(SeguridadApp seguridadApp, IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
            this._seguridadApp = seguridadApp;

        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusResponse>> Login([FromBody] LoginRequest data)
        {
            StatusReponse<UsuarioPersona> response = await _seguridadApp.ValidaUsuario(data);

            if (response.Success)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                   {
                       new Claim(ClaimTypes.NameIdentifier, response.Data.Persona.ToString()),
                   }),
                    Expires = DateTime.UtcNow.AddMonths(3).AddHours(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Data.token = tokenHandler.WriteToken(token);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("obtener-token-para-layherp/{idUsuario:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioPersona>> ObtenerTokenParaOtraAplicacion([FromRoute] int idUsuario)
        {
            StatusReponse<UsuarioPersona> response = await _seguridadApp.ObtenerUsuarioInternoPorId(idUsuario);

            if (response.Success)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                   {
                       new Claim(ClaimTypes.NameIdentifier, response.Data.Persona.ToString()),
                   }),
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Data.token = tokenHandler.WriteToken(token);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("CambioContrasena")]
        public async Task<ActionResult<StatusResponse>> CambioContrasena(MaestroPersonas data)
        {
            StatusResponse response = await _seguridadApp.CambioContrasena(data);
            return Ok(response);
        }

        [HttpPost]
        [Route("ValidaTokenListaNegra")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusResponse>> ValidaTokenListaNegra(string token)
        {
            StatusResponse response = await _seguridadApp.validarTokenEnListaNegra(token);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("GrabaTokenListaNegra")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusReponse<ListaNegra>>> SaveTokenListaNegra(ListaNegra entity)
        {
            StatusReponse<ListaNegra> response = await _seguridadApp.SaveListaNegra(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("RecuperarContrasena")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusResponse>> RecuperarContrasena([FromBody] InRecuperarContrasena data)
        {
            StatusResponse status = await _seguridadApp.RecuperarContrasena(data.NroDocumento);
            if (!status.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, status);
            return Ok(status);
        }

        [HttpPost]
        [Route("loginCliente")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusResponse>> LoginCliente([FromBody] UsuarioCliente data)
        {
            StatusReponse<UsuarioCliente> response = await _seguridadApp.ValidaUsuarioCliente(data);

            if (response.Success)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                   {
                       new Claim(ClaimTypes.NameIdentifier, response.Data.Usuario_id.ToString()),
                       new Claim(ClaimTypes.PrimaryGroupSid, response.Data.Cliente_id.ToString())
                   }),
                    Expires = DateTime.UtcNow.AddMonths(3).AddHours(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Data.Token = tokenHandler.WriteToken(token);
            }
            return Ok(response);
        }
    }
}
