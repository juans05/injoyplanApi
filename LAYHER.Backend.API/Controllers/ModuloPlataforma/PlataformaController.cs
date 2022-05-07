using CloudinaryDotNet;
using LAYHER.Backend.Application.ModuloPlataforma;
using LAYHER.Backend.Domain.ModuloPlataforma.DTO;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Controllers.ModuloPlataforma
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlataformaController : ControllerBase
    {

        private readonly PlataformaApp _PlataformaApp;
        private readonly IConfiguration _config;
        public static Cloudinary cloudinary;
        private IHostingEnvironment Environment;
        public PlataformaController(PlataformaApp plataformaApp, IConfiguration config, IHostingEnvironment _environment)
        {
            _PlataformaApp = plataformaApp;
            this._config = config.GetSection("RUTAS");

            Environment = _environment;
        }

        [HttpGet]
        [Route("ConsultaPlataforma")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Plataforma>>> List()
        {
            StatusReponse<List<Plataforma>> response = await _PlataformaApp.List();
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("grabarPlataforma")]
        public async Task<ActionResult<StatusReponse<Plataforma>>> Save(Plataforma entity)
        {

            var json = JsonSerializer.Serialize(entity);
            bool activoError = true;

            StatusResponse response = await _PlataformaApp.Save(entity);
            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }

            return Ok(response);
        }
    }
}
