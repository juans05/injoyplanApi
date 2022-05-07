using LAYHER.Backend.Application.ModuloProgramacionUnidad;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Controllers.ModuloProgramacionUnidad
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdjuntoController : ControllerBase
    {
    
        private readonly AdjuntoApp _adjuntoApp;
        private string _documentoFirmar;
        private string _induccionVisita;
        private string _guiaDespachoDevolucion;
        private string _mapas;
        private string _preventivaCovid;
        public AdjuntoController(AdjuntoApp adjuntoApp,
                                 IConfiguration config
                                )
        {
            _adjuntoApp = adjuntoApp;
          
            var rutas = config.GetSection("RUTAS");
            if (rutas != null)
            {
                this._documentoFirmar = rutas.GetValue<string>("DocumentosFirmar");
                this._induccionVisita = rutas.GetValue<string>("InduccionVisita");
                this._guiaDespachoDevolucion = rutas.GetValue<string>("GuiaDespachoDevolucion");
                this._mapas = rutas.GetValue<string>("Mapas");
                this._preventivaCovid = rutas.GetValue<string>("PreventivaCovid");
            }
        }

        [HttpGet]
        [Route("ListaDocumentoxAdjuntar")]
        public async Task<ActionResult<List<Adjunto>>> List(int id)
        {
            StatusReponse<List<Adjunto>> response = await _adjuntoApp.List(id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("DescargaDocumentosAdjuntos")]
        [AllowAnonymous]
        public async Task<FileContentResult> DescargaDocumentosAdjuntos(int id)
        {
            StatusReponse<ArchivoAdjunto> response = await _adjuntoApp.DescargaDocumentosAdjuntos(id);
            var data = System.IO.File.ReadAllBytes(response.Data.Ruta);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = response.Data.Nombre
            };
            return result;
        }

        [HttpGet]
        [Route("ListaDocumentosFormatos")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ArchivoAdjunto>>> ListaFormatos()
        {
            List<ArchivoAdjunto> lista = new List<ArchivoAdjunto>();

            string path = _documentoFirmar;
            string[] lst = System.IO.Directory.GetFiles(path);
            foreach (var item in lst)
            {
                ArchivoAdjunto data = new ArchivoAdjunto();
                data.Ruta = item;
                data.Nombre = item.Replace(path, "");
                lista.Add(data);
            }
            return lista;
        }

        [HttpGet]
        [Route("DescargaDocumentosFormatos")]
        [AllowAnonymous]
        public async Task<FileContentResult> DescargaDocumentosFormatos([FromQuery]ArchivoAdjunto entity)
        {
            var data = System.IO.File.ReadAllBytes(entity.Ruta);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = entity.Nombre
            };
            return result;
        }

        [HttpGet]
        [Route("DescargaInduccionVisita")]
        [AllowAnonymous]
        public async Task<FileContentResult> DescargaInduccionVisita()
        {
            string path = _induccionVisita;
            string[] lst = System.IO.Directory.GetFiles(path);
            string nombre = "";
            foreach (var item in lst)
            {
                nombre = item.Replace(path, "");
            }

            var data = System.IO.File.ReadAllBytes(lst[0]);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = nombre
            };
            return result;
        }

        [HttpGet]
        [Route("DescargaGuiaDespachoDevolucion")]
        [AllowAnonymous]
        public async Task<FileContentResult> DescargaGuiaDespachoDevolucion()
        {
            string path = _guiaDespachoDevolucion;
            string[] lst = System.IO.Directory.GetFiles(path);
            string nombre = "";
            foreach (var item in lst)
            {
                nombre = item.Replace(path, "");
            }

            var data = System.IO.File.ReadAllBytes(lst[0]);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = nombre
            };
            return result;
        }

        [HttpGet]
        [Route("DescargaMapaAlmacen")]
        [Produces("image/png", "image/webp", "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> DescargaMapaAlmacen(string almacen)
        {
            string path = _mapas;
            string[] lst = System.IO.Directory.GetFiles(path);
            string nombreArchivo = "";
            int index = -1;
            foreach (var item in lst)
            {
                string nombreExtension = item.Replace(path, "");
                string extension = nombreExtension.Replace(almacen,"");
                string nombre = nombreExtension.Replace(extension, "");
                index++;
                if (nombreExtension.Equals(almacen + extension))
                {
                    nombreArchivo = nombreExtension;
                    break; 
                }
            }
            
            if (string.IsNullOrEmpty(nombreArchivo)) 
            {
                return BadRequest("El almacen no tiene ubicacion asignada...");
            } 
            var data = System.IO.File.ReadAllBytes(lst[index]);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "Ubicacion del Almacen en " + nombreArchivo
            };

            return result;
        }

        [HttpGet]
        [Route("DescargaPreventivaCovid")]
        [AllowAnonymous]
        public async Task<FileContentResult> DescargaPreventivaCovid()
        {
            string path = _preventivaCovid;
            string[] lst = System.IO.Directory.GetFiles(path);
            string nombre = "";
            foreach (var item in lst)
            {
                nombre = item.Replace(path, "");
            }

            var data = System.IO.File.ReadAllBytes(lst[0]);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = nombre
            };
            return result;
        }

        [HttpGet]
        [Route("MuestraImagenMapa")]
        [AllowAnonymous]
        public IActionResult MuestraImagenMapa(string almacen)
        {
            string path = _mapas;
            string[] lst = System.IO.Directory.GetFiles(path);
            string nombreArchivo = "";
            int index = -1;
            foreach (var item in lst)
            {
                string nombreExtension = item.Replace(path, "");
                string extension = nombreExtension.Replace(almacen, "");
                string nombre = nombreExtension.Replace(extension, "");
                index++;
                if (nombreExtension.Equals(almacen + extension))
                {
                    nombreArchivo = nombreExtension;
                    break;
                }
            }

            if (string.IsNullOrEmpty(nombreArchivo))
            {
                return BadRequest("El almacen no tiene ubicacion asignada...");
            }

            var image = System.IO.File.OpenRead(path + nombreArchivo);
            return File(image, "image/jpeg");
        }
    }
}
