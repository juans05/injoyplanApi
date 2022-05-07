using LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Controllers.ModuloAdministracionUsuarioCliente
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly PerfilApp _perfilApp;

        public PerfilController(PerfilApp perfilApp)
        {
            _perfilApp = perfilApp;
        }

        [HttpGet]
        [Route("ListaPerfil")]
        public async Task<ActionResult<List<Perfil>>> List(int id)
        {
            StatusReponse<List<Perfil>> response = await _perfilApp.List(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaPerfilByDocumento")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Perfil>>> ListByDocumento(string documento)
        {
            StatusReponse<List<Perfil>> response = await _perfilApp.ListByDocumento(documento);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaPerfilPersona")]
        public async Task<ActionResult<List<PerfilPersona>>> ListPerfilPersona(int id)
        {
            StatusReponse<List<PerfilPersona>> response = await _perfilApp.ListPerfilPersona(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaPerfilPermiso")]
        public async Task<ActionResult<List<PerfilPermiso>>> ListPerfilPermiso(int IdPerfil, int IdPermiso)
        {
            StatusReponse<List<PerfilPermiso>> response = await _perfilApp.ListPerfilPermiso(IdPerfil,IdPermiso);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("GrabaPerfilPersona")]
        public async Task<ActionResult<StatusReponse<PerfilPersona>>> SavePerfilPersona(PerfilPersona entity)
        {
            StatusReponse<PerfilPersona> response = await _perfilApp.SavePerfilPersona(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }
    }
}
