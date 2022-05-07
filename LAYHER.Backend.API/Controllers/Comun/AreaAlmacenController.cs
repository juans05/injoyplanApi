using LAYHER.Backend.Application.Comun;
using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Controllers.Comun
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AreaAlmacenController : ControllerBase
    {
     
        private readonly AreaAlmacenApp _areaAlmacenApp;

        public AreaAlmacenController(AreaAlmacenApp areaAlmacenApp)
        {
            _areaAlmacenApp = areaAlmacenApp;
           
        }

        [HttpGet]
        [Route("ListaSede")]
        public async Task<ActionResult<List<RegionAlmacen>>> ListSede(int id)
        {
            StatusReponse<List<RegionAlmacen>> response = await _areaAlmacenApp.ListSede(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaAlmacen")]
        public async Task<ActionResult<List<AreaAlmacen>>> ListAlmacen(int id)
        {
            StatusReponse<List<AreaAlmacen>> response = await _areaAlmacenApp.ListAlmacen(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("Almacen/{idAlmacen}")]
        public async Task<ActionResult<Almacen>> ObtenerAlmacenPorId(string idAlmacen)
        {
            StatusReponse<Almacen> response = await _areaAlmacenApp.ObtenerAlmacenPorId(idAlmacen);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaTodosAlmacen")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AreaAlmacen>>> ListTodosAlmacenes()
        {
            StatusReponse<List<AreaAlmacen>> response = await _areaAlmacenApp.ListTodosAlmacenes();
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
    }
}
