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
    public class PermisoController : ControllerBase
    {

        private readonly PermisoApp _permisoApp;

        public PermisoController(PermisoApp permisoApp)
        {
            _permisoApp = permisoApp;
        }

        [HttpGet]
        [Route("ListaPermiso")]
        public async Task<ActionResult<List<Permiso>>> List(int id)
        {
            StatusReponse<List<Permiso>> response = await _permisoApp.List(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaPermisoPerfilPersona")]
        public async Task<ActionResult<List<PermisoPerfilPersona>>> ListPermisoPerfilPersona(int id)
        {
            StatusReponse<List<PermisoPerfilPersona>> response = await _permisoApp.ListPermisoPerfilPersona(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaPermisoProyectoPerfilPersona")]
        public async Task<ActionResult<List<PermisoPerfilPersona>>> ListPermisoProyectoPerfilPersona(int persona, int perfil, string afe, string documento, string nombre)
        {
            StatusReponse<List<PermisoPerfilPersona>> response = await _permisoApp.ListPermisoProyectoPerfilPersona(persona,perfil,afe,documento,nombre);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("GrabaPermisoPerfilPersona")]
        public async Task<ActionResult<StatusResponse>> SavePermisoPerfilPersona(PermisoPerfilPersona entity)
        {
            StatusResponse response = await _permisoApp.SavePermisoPerfilPersona(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

    }
}
