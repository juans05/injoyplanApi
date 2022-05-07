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
    public class AdministracionUsuarioClienteController : ControllerBase
    {
       
        private readonly AdministracionUsuarioClienteApp _administracionUsuarioClienteApp;

        public AdministracionUsuarioClienteController(AdministracionUsuarioClienteApp administracionUsuarioClienteApp)
        {
            _administracionUsuarioClienteApp = administracionUsuarioClienteApp;
           
        }

        [HttpGet]
        [Route("ListaProyectoxPersona")]
        public async Task<ActionResult<List<MaestroProyectos>>> ListProyectoxPersona(int persona, string proyectos, string afe, string localname, int zona, int offset, int fetch)
        {
            StatusReponse<List<MaestroProyectos>> response = await _administracionUsuarioClienteApp.ListByPersona(persona, proyectos, afe, localname, zona, offset, fetch);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("GrabaAdministracionUsuarioCliente")]
        public async Task<ActionResult<StatusResponse>> Save(PermisoPerfilPersona entity)
        {
            StatusResponse response = await _administracionUsuarioClienteApp.Save(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("ActualizaAdministracionUsuarioCliente")]
        public async Task<ActionResult<StatusResponse>> Update(PermisoPerfilPersona entity)
        {
            StatusResponse response = await _administracionUsuarioClienteApp.Update(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("InhabilitaHabilitaUsuarios")]
        public async Task<ActionResult<StatusResponse>> DisableEnablePersona(string documento, string estado)
        {
            StatusResponse response = await _administracionUsuarioClienteApp.DisableEnablePersona(documento, estado);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("EliminarUsuario")]
        public async Task<ActionResult<StatusResponse>> DeleteUsuario(string documento)
        {
            StatusResponse response = await _administracionUsuarioClienteApp.DeleteUsuario(documento);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }
    }
}
