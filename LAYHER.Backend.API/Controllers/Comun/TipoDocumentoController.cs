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
    public class TipoDocumentoController : ControllerBase
    {

        private readonly TipoDocumentoApp _tipoDocumentoApp;

        public TipoDocumentoController(TipoDocumentoApp tipoDocumentoApp)
        {
            _tipoDocumentoApp = tipoDocumentoApp;
           
        }

        [HttpGet]
        [Route("ListaTipoDocumento")]
        [AllowAnonymous]
        public async Task<ActionResult<List<TipoDocumento>>> List(int id, int activo)
        {
            StatusReponse<List<TipoDocumento>> response = await _tipoDocumentoApp.List(id, activo);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }
    }
}
