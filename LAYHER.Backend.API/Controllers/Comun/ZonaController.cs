using LAYHER.Backend.Application.Comun;
using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Controllers.Comun
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ZonaController : ControllerBase
    {

        private readonly ZonaApp _zonaApp;

        public ZonaController(ZonaApp zonaApp)
        {
            _zonaApp = zonaApp;
        
        }

        [HttpGet]
        [Route("ListaZona")]
        public async Task<ActionResult<List<Zona>>> List(int id)
        {
            StatusReponse<List<Zona>> response = await _zonaApp.List(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
        [HttpGet]
        [Route("ListDepartamento")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Zona>>> ListDepartamento()
        {
            StatusReponse<List<Zona>> response = await _zonaApp.ListDepartamento();
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
        [HttpGet]
        [Route("ListProvincia")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Zona>>> ListProvincia(string departamento)
        {
            StatusReponse<List<Zona>> response = await _zonaApp.ListProvincia(departamento);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
        [HttpGet]
        [Route("ListDistrito")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Zona>>> ListDistrito(string provincia)
        {
            StatusReponse<List<Zona>> response = await _zonaApp.ListDistrito(provincia);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
    }
}
