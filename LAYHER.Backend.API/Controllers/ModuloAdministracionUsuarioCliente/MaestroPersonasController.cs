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
    public class MaestroPersonasController : ControllerBase
    {
       
        private readonly MaestroPersonasApp _maestroPersonasApp;

        public MaestroPersonasController(MaestroPersonasApp maestroPersonasApp)
        {
            _maestroPersonasApp = maestroPersonasApp;

        }

        [HttpGet]
        [Route("ListaMaestroPersonas")]
        [AllowAnonymous]
        public async Task<ActionResult<List<MaestroPersonas>>> List(int id, int offset, int fetch)
        {
            StatusReponse<List<MaestroPersonas>> response = await _maestroPersonasApp.List(id,offset,fetch);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaClientes")]
        public async Task<ActionResult<List<MaestroPersonas>>> ListCliente()
        {
            StatusReponse<List<MaestroPersonas>> response = await _maestroPersonasApp.ListCliente();
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("GrabaMaestroPersonas")]
        public async Task<ActionResult<StatusReponse<MaestroPersonas>>> Save(MaestroPersonas entity)
        {
            StatusReponse<MaestroPersonas> response = await _maestroPersonasApp.Save(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }

        [HttpPost]
        [Route("ActualizaMaestroPersonas")]
        public async Task<ActionResult<StatusResponse>> Update(MaestroPersonas entity)
        {
            StatusResponse response = await _maestroPersonasApp.Update(entity);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response);
        }
    }
}
