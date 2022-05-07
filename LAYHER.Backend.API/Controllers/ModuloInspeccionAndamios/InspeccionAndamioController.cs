using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Application.Inspeccion;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using LAYHER.Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using LAYHER.Backend.API.Helpers;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;
using Microsoft.Reporting.NETCore;
using System.Reflection;
using NPOI.XSSF.UserModel;
using System.Net.Mime;

namespace LAYHER.Backend.API.Controllers.ModuloInspeccionAndamios
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InspeccionAndamioController : ControllerBase
    {
        private readonly string _rutaFolderRaiz;
        private readonly string _nombreFolderTmp;

        private readonly string _rutaFolderParaModulo;
        private readonly string _nombreFolderRegistroFotografico;
        private readonly string _nombreFolderPlantilla;

        private readonly IHostEnvironment _environment;

        private readonly InspeccionAndamioApp _inspeccionAndamioApp;

        public InspeccionAndamioController(InspeccionAndamioApp InspeccionAndamioApp,  IHostEnvironment environment)
        {
            _inspeccionAndamioApp = InspeccionAndamioApp;
            _environment = environment;

            _environment = environment;

            this._rutaFolderRaiz = string.Format("{0}{1}contenido", this._environment.ContentRootPath, Path.DirectorySeparatorChar);
            this._nombreFolderTmp = "tmp";

            this._rutaFolderParaModulo = string.Format("{0}{1}inspeccion_andamios", this._rutaFolderRaiz, Path.DirectorySeparatorChar);
            this._nombreFolderRegistroFotografico = "registro_fotografico";
            this._nombreFolderPlantilla = "plantilla";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<List<InspeccionAndamio>>> ListarInspeccionAndamio(
            string nombreProyecto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int offset,
            int fetch,
            bool? verHistorial,
            bool modoHistorico = false,
            int cantidadAnios = 3)
        {
            StatusReponse<List<InspeccionAndamio>> response = await _inspeccionAndamioApp.ListarInspeccionAndamio(nombreProyecto, fechaInicio, fechaFin, offset, fetch, verHistorial, modoHistorico, cantidadAnios);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaPreguntas")]
        public async Task<ActionResult<List<PreguntaInspeccion>>> ListarPreguntaInspeccionAndamio(int tipoAndamio_id, int? subTipoAndamio_id = null)
        {
            StatusReponse<List<PreguntaInspeccion>> response = await _inspeccionAndamioApp.ListarPreguntaInspeccionAndamio(tipoAndamio_id, subTipoAndamio_id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaSubTipoAndamio")]
        public async Task<ActionResult<List<SubTipoAndamio>>> ListarSubTipoAndamios()
        {
            StatusReponse<List<SubTipoAndamio>> response = await _inspeccionAndamioApp.ListarSubTipoAndamios();
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaTipoAndamios")]
        public async Task<ActionResult<List<TipoAndamio>>> ListarTipoAndamio()
        {
            StatusReponse<List<TipoAndamio>> response = await _inspeccionAndamioApp.ListarTipoAndamio();
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ListaClientes")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Cliente>>> ListarCliente(string documentoUsuario)
        {
            StatusReponse<List<Cliente>> response = await _inspeccionAndamioApp.ListarCliente(documentoUsuario);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult<InspeccionAndamio>> Obtener(int inspeccionAndamio_id)
        {
            StatusReponse<InspeccionAndamio> response = await _inspeccionAndamioApp.Obtener(inspeccionAndamio_id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            if (response.Data.ListaRegistroFotografico != null)
            {

            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("CheckList")]
        public async Task<ActionResult<OutCheckList>> ObtenerCheckList(int checkList_id)
        {
            StatusReponse<OutCheckList> response = await _inspeccionAndamioApp.ObtenerCheckList(checkList_id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }
        [HttpGet]
        [Route("configuracion-app/{usuario_id}")]
        public async Task<ActionResult<OutConfiguracionApp>> ObtenerConfiguracionApp(int usuario_id)
        {
            StatusReponse<OutConfiguracionApp> response = await _inspeccionAndamioApp.ObtenerConfiguracionApp(usuario_id);
            if (!response.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }
        [HttpPost]
        [Route("configuracion-app")]
        public async Task<ActionResult<StatusResponse>> CreateUpdateConfiguracionApp(ConfiguracionApp configuracion)
        {
            StatusResponse response = new StatusResponse();
            int idUsuario = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);
            if (idUsuario == 0)
            {
                response = new StatusResponse(false, "Usuario no válido", "");
            }
            response = await _inspeccionAndamioApp.CreateUpdateConfiguracionApp(configuracion, idUsuario);
            return Ok(response);
        }
        [HttpPost]
        [Route("Registro")]
        public async Task<ActionResult<StatusReponse<InspeccionAndamio>>> Registrar(InspeccionAndamio inspeccionAndamio)
        {
            StatusReponse<InspeccionAndamio> response = new StatusReponse<InspeccionAndamio>();
            List<string> erroresArchivo = new List<string>();
            string errorArchivo = "";
            string folderRegistroFotografico = this._rutaFolderParaModulo + Path.DirectorySeparatorChar + this._nombreFolderRegistroFotografico + Path.DirectorySeparatorChar;
            int idUsuario = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);
            int idCliente = Helpers.Utils.GetLoggedClientId(HttpContext.User.Identity);
            if (idUsuario == 0)
            {
                response = new StatusReponse<InspeccionAndamio>(false, "Usuario no válido", "");
            }
            else
            {
                inspeccionAndamio.IdUsuario = idUsuario;
                inspeccionAndamio.Cliente_id = idCliente;

                response = await _inspeccionAndamioApp.Registrar(inspeccionAndamio);

                //Mover archivos/imágenes temporales
                if (response.Success)
                {
                    if (inspeccionAndamio.ListaRegistroFotografico != null && inspeccionAndamio.ListaRegistroFotografico.Count > 0)
                    {
                        foreach (RegistroFotografico foto in inspeccionAndamio.ListaRegistroFotografico)
                        {
                            if (!string.IsNullOrEmpty(foto.NombreArchivoTemporal) && foto.NombreArchivoTemporal.StartsWith("/tmp/"))
                            {
                                try
                                {
                                    string fileTmpPath = this._rutaFolderRaiz + Path.DirectorySeparatorChar + foto.NombreArchivoTemporal;
                                    string fileName = Path.GetFileName(fileTmpPath);
                                    string filePathTarget = folderRegistroFotografico + fileName;
                                    System.IO.File.Move(fileTmpPath, filePathTarget);
                                }
                                catch (Exception ex)
                                {
                                    errorArchivo = string.Format("Error al guardar el archivo {0}", foto.Nombre);
                                   
                                    erroresArchivo.Add(errorArchivo);
                                }
                            }
                        }
                    }
                    if (inspeccionAndamio.ListaRegistroFotograficoEliminado != null && inspeccionAndamio.ListaRegistroFotograficoEliminado.Count > 0)
                    {
                        foreach (RegistroFotografico foto in inspeccionAndamio.ListaRegistroFotograficoEliminado)
                        {
                            string filePath = "";
                            if (foto.Nombre.StartsWith("/tmp/"))
                                filePath = this._rutaFolderRaiz + Path.DirectorySeparatorChar + foto.Nombre;
                            else
                                filePath = folderRegistroFotografico + foto.Nombre;

                            try
                            {
                                System.IO.File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                errorArchivo = string.Format("Error al eliminar el archivo {0}", foto.Nombre);
                                
                                erroresArchivo.Add(errorArchivo);
                            }
                        }
                    }
                    if (erroresArchivo.Count > 0)
                    {
                        Dictionary<string, List<string>> errores = new Dictionary<string, List<string>>();
                        errores.Add("RegistroFotografico", erroresArchivo);
                        response = new StatusReponse<InspeccionAndamio>("Se guardo la información de manera satisfactoria, pero ocurrió un error al guardar los archivos", errores) { Success = true };
                    }
                }
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("RegistroCheckList")]
        public async Task<ActionResult<StatusResponse>> RegistrarCheckList(CheckList checkList)
        {
            StatusResponse response = null;
            int idUsuario = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);
            if (idUsuario == 0)
            {
                response = new StatusReponse<InspeccionAndamio>(false, "Usuario no válido", "");
            }
            else
            {
                checkList.IdUsuario = idUsuario;
                response = await _inspeccionAndamioApp.RegistrarCheckList(checkList);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("ActualizaHistorico")]
        public async Task<ActionResult<StatusResponse>> ActualizarHistorico(List<int> listaId)
        {
            StatusResponse response = null;
            int idUsuario = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);
            if (idUsuario == 0)
            {
                response = new StatusReponse<InspeccionAndamio>(false, "Usuario no válido", "");
            }
            else
            {
                if (listaId == null || listaId.Count == 0)
                    response = new StatusReponse<InspeccionAndamio>(false, "Lista de id vacía", "");
                else
                    response = await _inspeccionAndamioApp.ActualizarHistorico(listaId, idUsuario);
            }
            return Ok(response);
        }

        [Route("upload/single")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<StatusResponse>> PostUploadSingleFile(IFormFile file)
        {
            StatusReponse<string> response = new StatusReponse<string>();
            if (file == null || file.Length == 0)
            {
                return new StatusResponse(false, string.Format("El archivo está vacio. Cargar un archivo válido"), "");
            }

            string internalFileNameWithoutExt = Guid.NewGuid().ToString();
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExt = Path.GetExtension(file.FileName);
            string newFileName = internalFileNameWithoutExt + fileExt;

            try
            {
                var filePath = this._rutaFolderRaiz + Path.DirectorySeparatorChar + this._nombreFolderTmp + Path.DirectorySeparatorChar + newFileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                response.Success = true;
                response.Data = "/tmp/" + newFileName;
            }
            catch (System.Exception ex)
            {
              
                return new StatusResponse(false, string.Format("Error al guardar el archivo {0}", file.FileName), "");
            }
            return await Task.Run(() => response);
        }

        [HttpGet("download/single")]
        public async Task<ActionResult> DownloadSingleFile([FromQuery] string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                return NotFound();

            string fileAbsolutePath = "";
            string folderRegistroFotografico = this._rutaFolderParaModulo + Path.DirectorySeparatorChar + this._nombreFolderRegistroFotografico + Path.DirectorySeparatorChar;
            string fileName = "";
            if (nombre.StartsWith("/tmp/"))
            {
                fileAbsolutePath = this._rutaFolderRaiz + Path.DirectorySeparatorChar + nombre;
                fileName = nombre.Substring(5);
            }
            else
            {
                RegistroFotografico foto = (await _inspeccionAndamioApp.Obtener(nombre)).Data;
                if (foto == null || foto.RegistroFotografico_id == 0)
                    return NotFound();

                fileAbsolutePath = folderRegistroFotografico + nombre;
                fileName = foto.NombreOriginal;
            }

            if (!System.IO.File.Exists(fileAbsolutePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(fileAbsolutePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            //Importante para que por el cliente (navegador) pueda extraer el nombre del archivo que está enviando el servidor
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
                contentType = "application/octet-stream";

            return File(memory, contentType, fileName);
        }

        [HttpGet]
        [Route("DescargaExcel")]
        public async Task<FileContentResult> DescargarEnExcel(
            string nombreProyecto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            bool? verHistorial)
        {
            StatusReponse<List<InspeccionAndamio>> status = await _inspeccionAndamioApp.ListarInspeccionAndamio(nombreProyecto, fechaInicio, fechaFin, 0, int.MaxValue, verHistorial);

            if (!status.Success)
            {
                throw new Exception("No se pudo descargar el archivo");
            }

            if (status.Data.Count == 0)
            {
                throw new Exception("No hay registros para descargar");
            }

            List<InspeccionAndamio> lista = status.Data;
            int totalRegistros = lista.Count;
            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = string.Format("SA-Reporte-{0}.xlsx", DateTime.Now.ToString("ddMMMyyyy-HH-mm"));
            byte[] bytes = null;

            //string DateCellFormat = "dd/mm/yyyy";
            //// number formats
            //string numberWithoutDecimalFormat = "#,##0_)";
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;
            string hostingRoot = string.Empty;
            //string webConfigReportTemplatePath = ConfigurationManager.AppSettings["reportTemplatePath"];
            string filePath = string.Empty;
            IWorkbook _workbook;
            ISheet _sheet;
            IRow sheetRow = null;
            int rowWithData = 7;
            FileStream fileStream = null;



            filePath = string.Format("{1}{0}{2}{0}{3}", Path.DirectorySeparatorChar, this._rutaFolderParaModulo, this._nombreFolderPlantilla, "plantilla-reporte.xlsx");

            try
            {
                using (fileStream = new FileStream(filePath, FileMode.Open))
                {
                    //_workbook = WorkbookFactory.Create(fileStream);
                    _workbook = new XSSFWorkbook(fileStream);
                    ICellStyle _dateCellStyle = _workbook.CreateCellStyle();
                    _dateCellStyle.DataFormat = _workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

                    _sheet = _workbook.GetSheet("Reporte");
                    for (int i = 0; i < totalRegistros; i++)
                    {
                        sheetRow = _sheet.CreateRow(rowWithData);

                        sheetRow.CreateCell((int)ColumnaExcel.Item).SetCellValue(i + 1);
                        sheetRow.CreateCell((int)ColumnaExcel.Empresa).SetCellValue(lista[i].NombreCliente);
                        sheetRow.CreateCell((int)ColumnaExcel.Proyecto).SetCellValue(lista[i].Proyecto);
                        sheetRow.CreateCell((int)ColumnaExcel.FrenteTrabajo).SetCellValue(lista[i].ZonaTrabajo);
                        sheetRow.CreateCell((int)ColumnaExcel.TipoAndamio).SetCellValue(lista[i].TipoAndamio_Nombres);
                        sheetRow.CreateCell((int)ColumnaExcel.MarcaAndamio).SetCellValue(lista[i].MarcaAndamio);

                        ICell cellSobrecargaUso = sheetRow.CreateCell((int)ColumnaExcel.SobrecargaUso);
                        cellSobrecargaUso.SetCellValue(lista[i].SobreCargaUso);

                        sheetRow.CreateCell((int)ColumnaExcel.EncargadoLiderFrente).SetCellValue(lista[i].Responsable);//Ingreso en el formulario como texto libre
                        sheetRow.CreateCell((int)ColumnaExcel.SupervisorQueInspecciono).SetCellValue(lista[i].NombreUsuario);

                        if (lista[i].FechaCreacion.HasValue)
                        {
                            ICell cellNotified = sheetRow.CreateCell((int)ColumnaExcel.Fecha);
                            cellNotified.SetCellValue(lista[i].FechaCreacion.Value);
                            cellNotified.CellStyle = _dateCellStyle;
                        }

                        sheetRow.CreateCell((int)ColumnaExcel.Observaciones).SetCellValue(lista[i].Observacion);

                        rowWithData++;
                    }

                    using (var memoryStream = new MemoryStream()) //creating memoryStream
                    {
                        _workbook.Write(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            finally
            {

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    fileStream = null;
                }
            }

            var cd = new ContentDisposition
            {
                Inline = false,
                FileName = fileName
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(bytes, mimeType);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("DescargaInformeDigital/{idInspeccion}/pdf")]
        public async Task<FileContentResult> DescargarInformeEnPDF([FromRoute] int idInspeccion)
        {
            string renderFormat = "PDF";
            string mimeType = "application/pdf";
            string extension = ".pdf";
            string fileName = "Inspeccion-" + idInspeccion + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + extension;
            byte[] bytes = await generarReporte(idInspeccion, renderFormat);

            Response.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}", fileName));

            return File(bytes, mimeType);
        }

        [HttpGet]
        [Route("DescargaInformeDigital/{idInspeccion}/word")]
        public async Task<FileContentResult> DescargarInformeEnWord([FromRoute] int idInspeccion)
        {
            string renderFormat = "WORDOPENXML";
            string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string extension = ".docx";
            string fileName = "Inspeccion-" + idInspeccion + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + extension;
            byte[] bytes = await generarReporte(idInspeccion, renderFormat);


            Response.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}", fileName));

            return File(bytes, mimeType);
        }

        private async Task<byte[]> generarReporte(int idInspeccion, string renderFormat)
        {
            string folderRegistroFotografico = this._rutaFolderParaModulo + Path.DirectorySeparatorChar + this._nombreFolderRegistroFotografico + Path.DirectorySeparatorChar;
            StatusReponse<ReporteInspeccion> response = await _inspeccionAndamioApp.ObtenerParaReporte(idInspeccion, folderRegistroFotografico);

            using var report = new LocalReport();
            report.EnableExternalImages = true;
            var parameters = new[] { new ReportParameter("Cliente", response.Data.NombreCliente) };
            using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("LAYHER.Backend.API.contenido.inspeccion_andamios.plantilla.InformeDigital.rdlc");
            report.LoadReportDefinition(rs);

            //get Inspeccion Collections
            var inspeccion = new OutReporteInspeccion { 
                InspeccionAndamio_id = response.Data.InspeccionAndamio_id,
                EstadoInspeccionAndamio_id = response.Data.EstadoInspeccionAndamio_id,
                Cliente_id = response.Data.EstadoInspeccionAndamio_id,
                Proyecto = response.Data.Proyecto,
                Direccion = response.Data.Direccion,
                ZonaTrabajo = response.Data.ZonaTrabajo,
                Responsable = response.Data.Responsable,
                Cargo = response.Data.Cargo,
                MarcaAndamio = response.Data.MarcaAndamio,
                SobreCargaUso = response.Data.SobreCargaUso,
                Observacion = response.Data.Observacion,
                IdUsuarioCreacion = response.Data.IdUsuarioCreacion,
                IdUsuarioEdicion = response.Data.IdUsuarioEdicion,
                FechaCreacion = response.Data.FechaCreacion,
                FechaEdicion = response.Data.FechaEdicion,
                NombreCliente = response.Data.NombreCliente,
                Estado = response.Data.Estado,
                NombreUsuarioCreacion = response.Data.NombreUsuarioCreacion,
                TipoAndamio_Nombres = response.Data.TipoAndamio_Nombres,
            };

            List<OutReporteInspeccion> lista = new List<OutReporteInspeccion>();
            lista.Add(inspeccion);

            report.DataSources.Add(new ReportDataSource("DataSetDatosInspeccion", lista));
            report.DataSources.Add(new ReportDataSource("DataSetCheckList", response.Data.ListaCumplimiento));
            report.DataSources.Add(new ReportDataSource("Fotos", response.Data.Fotos));
            report.SetParameters(parameters);

            var pdf = report.Render(renderFormat);
            return pdf;
        }
    }
}
