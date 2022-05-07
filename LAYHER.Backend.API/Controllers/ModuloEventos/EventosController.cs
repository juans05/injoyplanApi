using LAYHER.Backend.Application.ModuloEventos;
using LAYHER.Backend.Domain.ModuloEventos.DTO;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAYHER.Backend.API.Controllers.ModuloEventos
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        // GET: api/<EventosController>
       
        private readonly EventosApp _webShopApp;
        private readonly IConfiguration _config;
        public static Cloudinary cloudinary;
        private IHostingEnvironment EnvironmentH;
        public static IWebHostEnvironment _environment;

        public EventosController(EventosApp EventosApp,  IConfiguration config, IWebHostEnvironment _Environment, IHostingEnvironment _EnvironmentHosting)
        {
            _webShopApp = EventosApp;
            this._config = config.GetSection("RUTAS");
          
            _environment = _Environment;
            EnvironmentH = _EnvironmentHosting;
        }

        [HttpPost]
        [Route("AdminConsultaEvento")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ListarEventosAdmin>>> List(ListarEventoAdmin entity)
        {
            //int usuarioId = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);

            StatusReponse<List<ListarEventosAdmin>> response = await _webShopApp.AdminList(entity.perfil, entity.usuario, entity.Departamento, entity.Provincia, entity.distrito, entity.titulo);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }


        [HttpGet]
        [Route("EditEvento")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ListarEventosAdmin>>> editEvento(int id)
        {
            //int usuarioId = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);

            StatusReponse<Eventos> response = await _webShopApp.editEventos(id);
            StatusReponse<List<Eventos_Fecha>> responseListEventoFecha = await _webShopApp.ListarEventoFecha(id);
            StatusReponse<List<Eventos_Categoria>> responseEventoCategoria = await _webShopApp.Listar_EventoCategoria(id);
            StatusReponse<List<Evento_plataforma>> responseEventoPlataforma = await _webShopApp.Listar_EventoPlataforma(id);
            StatusReponse<List<EventoImagen>> responseEventoImagen = await _webShopApp.Listar_EventoImagen(id);
            StatusReponse<List<Evento_Entradas>> responseEventoEntradas = await _webShopApp.Listar_EventoEntradas(id);
            response.Data.EventoFechas = ((responseListEventoFecha.Data!= null ) ? responseListEventoFecha.Data : null);
            response.Data.EventoCategoria = ((responseEventoCategoria.Data != null) ? responseEventoCategoria.Data : null);
            response.Data.EventoPlataforma = ((responseEventoPlataforma.Data != null) ? responseEventoPlataforma.Data : null); 
            response.Data.EventoImagen = ((responseEventoImagen.Data != null) ? responseEventoImagen.Data : null);
            response.Data.EventoEntradas = ((responseEventoEntradas.Data != null) ? responseEventoEntradas.Data : null);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("DeleteEvento")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusReponse<Eventos>>> delete(Eventos entity)
        {
            StatusResponse response = new StatusResponse();
            StatusResponse responseEliminar = await _webShopApp.DeleteEventoCategoria(entity.id);
            if (responseEliminar.Success)
            {
                StatusResponse responseEliminarEntradas = await _webShopApp.DeleteEventoEntradas(entity.id);
                if (responseEliminarEntradas.Success)
                {
                    StatusResponse responseEliminarPlataforma = await _webShopApp.DeleteEventoPlataforma(entity.id);
                    if (responseEliminarPlataforma.Success)
                    {
                        StatusResponse responseEliminarEventoFecha = await _webShopApp.DeleteEventoFecha(entity.id);
                        if (responseEliminarEventoFecha.Success)
                        {
                            response = await _webShopApp.delete(entity);
                            if (!response.Success)
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError, responseEliminarEventoFecha);
                            }
                                
                        }
                    }

                   
                }

                return Ok(response);

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, responseEliminar);
            }
        }


        [HttpPost]
        [Route("ActualizarEvento")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusReponse<Eventos>>> update(Eventos entity)
        {

            var json = JsonSerializer.Serialize(entity);
            bool activoError = true;

            StatusResponse response = await _webShopApp.update(entity);
            if (response.Success)
            {
                StatusResponse responseEliminar = await _webShopApp.DeleteEventoCategoria(entity.id);
                 if (responseEliminar.Success)
                {
                    
                    StatusResponse response1 = await _webShopApp.SaveEventoCategoria(entity.EventoCategoria);
                    if (response1.Success)
                    {
                        StatusResponse responseEliminarEntradas = await _webShopApp.DeleteEventoEntradas(entity.id);
                        if (responseEliminarEntradas.Success)
                        {
                            if (entity.EventoPlataforma.Count > 0)
                            {
                                entity.EventoEntradas.ForEach(x => x.idEvento = entity.id);
                            }
                            
                            StatusResponse response2 = await _webShopApp.SaveEventoEntradas(entity.EventoEntradas);

                            StatusResponse responseEliminarPlataforma = await _webShopApp.DeleteEventoPlataforma(entity.id);
                            if (responseEliminarPlataforma.Success)
                            {
                                if (entity.EventoPlataforma.Count>0)
                                {
                                    entity.EventoPlataforma.ForEach(x => x.idEvento = entity.id);
                                }
                                
                                StatusResponse response3 = await _webShopApp.SaveEventoPlataforma(entity.EventoPlataforma);
                                if (response3.Success)
                                {
                                    StatusResponse responseEliminarEventoFecha = await _webShopApp.DeleteEventoFecha(entity.id);
                                    if (responseEliminarEventoFecha.Success)
                                    {
                                        if (entity.EventoFechas.Count > 0)
                                        {
                                            entity.EventoFechas.ForEach(x => x.idEvento = entity.id);

                                        }
                                        
                                        StatusResponse response4 = await _webShopApp.SaveEventoFecha(entity.EventoFechas,true);
                                        if (!response4.Success)
                                        {
                                            return StatusCode(StatusCodes.Status500InternalServerError, response4);
                                        }
                                    }
                                    else {
                                        return StatusCode(StatusCodes.Status500InternalServerError, responseEliminarEventoFecha);
                                    }                                  
                                }
                                else
                                {
                                    return StatusCode(StatusCodes.Status500InternalServerError, response3);

                                }
                            }
                            else
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError, responseEliminarPlataforma);
                            }
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, responseEliminarEntradas);
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, response1);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, responseEliminar);
                }

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }

            return Ok(response);
        }


        [HttpPost]
        [Route("subirImagen")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusReponse<Eventos>>> Upload([FromForm]  EventoImagen eventoImagen) {

            try
            {
                if (eventoImagen.imagen.Length > 0)
                {
                    var path = "D:\\";//Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));
                    //var CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var filePath = "";
                    if (!Directory.Exists(path + "\\contenido\\Imagen\\"))
                    {
                        Directory.CreateDirectory(path + "\\contenido\\Imagen\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + "\\contenido\\Imagen\\" + eventoImagen.imagen.FileName))
                    {
                      
                        await eventoImagen.imagen.CopyToAsync(fileStream);
                        fileStream.Flush();
                        filePath = path + "\\contenido\\Imagen\\" + eventoImagen.imagen.FileName;
                       

                    }

                    Account account = new Account(
                                                      "do4rokki9",
                                                      "216776255384154",
                                                       "KlsdZdmXNWHaxA0rthQ_S4bMk1U");

                    cloudinary = new Cloudinary(account);
                    cloudinary.Api.Secure = true;
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(filePath)
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    eventoImagen.principal = 1;
                    eventoImagen.tipo = "img";
                    eventoImagen.consecutivoImg = 1;
                    eventoImagen.url = uploadResult.SecureUrl.ToString();
                    StatusResponse response4 = await _webShopApp.SaveEventoAdjunto(eventoImagen);
                    if (!response4.Success)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, response4);
                    }

                    return Ok(new { count = eventoImagen.imagen.Length, filePath, tipo = "OK", mensaje = "OK" });
                }
                else {
                    var filePath = "";
                    return Ok(new { count = 0, filePath, tipo = "ERROR", mensaje = "No hay Imagen" });
                }
            }
            catch (Exception ex)
            {
                var filePath = "";
                return Ok(new { count = 0, filePath, tipo = "ERROR", mensaje = ex.Message.ToString() });
            }

            //string wwwPath = this.Environment.WebRootPath;
            //string contentPath = this.Environment.ContentRootPath;

          
            //return Ok("ok");
            //long size = eventoImagen.imagen.Sum(f => f.Length);

            // full path to file in temp location
            //var filePath = Path.GetTempFileName();

            //if (eventoImagen.imagen.Length > 0)
            //{
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await eventoImagen.imagen.CopyToAsync(stream);
            //    }
            //}

            //// process uploaded files
            //// Don't rely on or trust the FileName property without validation.

            //return Ok(new { count = eventoImagen.imagen.Length, filePath });
        }

      
        [HttpPost]
        [Route("GrabaEvento")]
        [AllowAnonymous]
        public async Task<ActionResult<StatusReponse<Eventos>>> Save(Eventos entity)
        {

            var json = JsonSerializer.Serialize(entity);
            bool activoError = true;

            StatusReponse<Eventos> response = await _webShopApp.Save(entity);

            StatusResponse response1 = new StatusResponse();
            if (response.Success)
            {
               
                entity.EventoCategoria.ForEach((Action<Eventos_Categoria>)(x =>
                {
                    x.id_Eventos = response.Data.id;
                  
                }));
                response1 = await _webShopApp.SaveEventoCategoria(entity.EventoCategoria);
                if (response1.Success)
                {
                    entity.EventoEntradas.ForEach((Action<Evento_Entradas>)(x =>
                    {
                        x.idEvento = response.Data.id;

                    }));
                    StatusResponse response2 = await _webShopApp.SaveEventoEntradas(entity.EventoEntradas);
                    if (response2.Success)
                    {
                        entity.EventoPlataforma.ForEach((Action<Evento_plataforma>)(x =>
                        {
                            x.idEvento = response.Data.id;

                        }));
                        StatusResponse response3 = await _webShopApp.SaveEventoPlataforma(entity.EventoPlataforma);
                        if (response3.Success)
                        {
                            entity.EventoFechas.ForEach((Action<Eventos_Fecha>)(x =>
                            {
                                x.idEvento = response.Data.id;

                            }));
                            StatusResponse response4 = await _webShopApp.SaveEventoFecha(entity.EventoFechas,false);
                            if (!response4.Success)
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError, response4);
                            } 
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, response3);

                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, response2);

                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response1);

                }
            } else {
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            }

            return Ok(response);
        }
    }
}
