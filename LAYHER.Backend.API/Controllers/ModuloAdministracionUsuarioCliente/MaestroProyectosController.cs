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
    public class MaestroProyectosController : ControllerBase
    {
      
        private readonly MaestroProyectosApp _maestroProyectosApp;

        public MaestroProyectosController(MaestroProyectosApp maestroProyectosApp)
        {
            _maestroProyectosApp = maestroProyectosApp;
           
        }

        [HttpGet]
        [Route("ListaMaestroProyectos")]
        public async Task<ActionResult<List<MaestroProyectos>>> List(string id, int offset, int fetch)
        {
            StatusReponse<List<MaestroProyectos>> response = await _maestroProyectosApp.List(id, offset, fetch);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaProyectoCliente")]
        public async Task<ActionResult<List<MaestroProyectos>>> ListProyectoCliente(string id)
        {
            StatusReponse<List<MaestroProyectos>> response = await _maestroProyectosApp.ListProyectoCliente(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
    }
}
