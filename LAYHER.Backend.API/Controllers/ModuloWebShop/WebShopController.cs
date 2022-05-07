using LAYHER.Backend.Application.ModuloWebShop;
using LAYHER.Backend.Domain.ModuloProductos.DTO;
using LAYHER.Backend.Domain.ModuloWebShop.DTO;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAYHER.Backend.API.Controllers.ModuloWebShop
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WebShopController : ControllerBase
    {
    
        private readonly WebShopApp _webShopApp;
        private readonly IConfiguration _config;
        public WebShopController(WebShopApp webShopApp,  IConfiguration config)
        {
            _webShopApp = webShopApp;
            this._config = config.GetSection("RUTAS");

        }

       


        [HttpPost]
        [Route("grabarWebApp")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusResponse>> Save(PedidoWebShop entity)
        {

            var json = JsonSerializer.Serialize(entity);
            StatusResponse response = await _webShopApp.SavePersona(entity.persona);
            bool activoError = true;
            if (response.Success)
            {

                var existeServicio = entity.Productos.Where(x => x.esServicio == true).ToList();
                if (existeServicio.Count() > 0)
                {
                    StatusResponse response3 = await _webShopApp.SaveFacturacion(entity);
                    if (!response3.Success)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, response3);
                        activoError = false;
                    }
                }
                else
                {
                    StatusResponse response2 = await _webShopApp.SavePreGuia(entity.Productos, entity.persona.Documento, json);
                    if (response2.Success)
                    {

                        StatusResponse response3 = await _webShopApp.SaveFacturacion(entity);
                        if (!response3.Success)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, response3);
                            activoError = false;
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, response2);
                        activoError = false;
                    }
                }


            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
                activoError = false;
            }

            if (!activoError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }

            response.Title = "Registro Correcto";
            return Ok(response);
            //if (!response.Success)
            //    return StatusCode(StatusCodes.Status500InternalServerError, response);
            //return Ok(response);
        }
    }
}
