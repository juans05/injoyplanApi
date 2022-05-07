using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloProgramacionUnidad
{
    public class FormularioApp : BaseApp<FormularioApp>
    {
        private IFormularioRepository _formularioRepository;
        private IProgramacionUnidadRepository _programacionUnidadRepository;
        private string _mapas;
        public FormularioApp(IFormularioRepository formularioRepository,
                             IProgramacionUnidadRepository programacionUnidadRepository,
                             ILogger<BaseApp<FormularioApp>> logger) : base()
        {
            this._formularioRepository = formularioRepository;
            this._programacionUnidadRepository = programacionUnidadRepository;
        }

        public async Task<StatusResponse> Save(Formulario entity, string ruta)
        {
            StatusReponse<Formulario> formulario = new StatusReponse<Formulario>() { Success = false, Title = "" };
            StatusReponse<AdjuntoFormulario> adjunto = new StatusReponse<AdjuntoFormulario>() { Success = false, Title = "" };
            StatusReponse<ArchivoAdjunto> archivo = new StatusReponse<ArchivoAdjunto>() { Success = false, Title = "" };
            StatusReponse<FormularioConsideracion> consideracion = new StatusReponse<FormularioConsideracion>() { Success = false, Title = "" };
            StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            StatusResponse programacion = new StatusResponse() { Success = false, Title = "", Detail = "" };

            try
            {
                #region Hansel: Quitar por marcha blanca
                //if (entity.Archivo is null) {
                //    respuesta.Detail = "Falta adjuntar el/los archivos...";
                //    respuesta.Success = false;
                //    return respuesta;
                //}
                //if (entity.IdAdjuntoSeleccion is null)
                //{
                //    respuesta.Detail = "Falta seleccionar el/los tipos de archivos que se envían...";
                //    respuesta.Success = false;
                //    return respuesta;
                //}
                //if (entity.Archivo.Count < 1 || entity.IdAdjuntoSeleccion.Count < 1)
                //{
                //    respuesta.Detail = "Falta adjuntar o seleccionar los archivos...";
                //    respuesta.Success = false;
                //    return respuesta;
                //}
                #endregion

                #region Hansel: Por marcha blanca

                if (entity.Archivo != null && entity.Archivo.Count == 0 && (entity.IdAdjuntoSeleccion == null || entity.IdAdjuntoSeleccion.Count > 0))
                {
                    respuesta.Detail = "Usted ha indicado que cargará algunos tipos de documento, sin embargo no ha cargado ningún archivo";
                    respuesta.Success = false;
                    return respuesta;
                }

                if ((entity.Archivo == null || entity.Archivo.Count > 0) && entity.IdAdjuntoSeleccion != null && entity.IdAdjuntoSeleccion.Count == 0)
                {
                    respuesta.Detail = "Debe indicar que tipo de documentos son los que archivos que está cargando";
                    respuesta.Success = false;
                    return respuesta;
                }

                #endregion

                ProgramacionUnidad validaProgramacion = await _programacionUnidadRepository.ProgramacionUnidadTiempoPorId(entity.ProgramacionUnidad);
                if (validaProgramacion.IdFormulario == 0)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        if (entity.EsTransportistaEncargado)
                        {
                            entity.IdTipoDocumentoEncargado = entity.IdTipoDocumentoTransportista;
                            entity.DocumentoEncargado = entity.DocumentoTransportista;
                            entity.NombreEncargado = entity.NombreTransportista;
                            entity.SctrEncargado = entity.SctrTransportista;
                        }
                        formulario = await _formularioRepository.Save(entity);
                        if (formulario.Success)
                        {
                            respuesta.Detail = "Formulario registrado.";
                            tran1.Complete();

                        }
                    }
                    if (formulario.Success)
                    {
                        using (var tran2 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            programacion = await _programacionUnidadRepository.UpdateFormularioProgramacion(entity.ProgramacionUnidad, formulario.Data.IdFormulario);
                            if (programacion.Success)
                            {
                                if (entity.IdAdjuntoSeleccion != null)
                                {
                                    foreach (var iAdjunto in entity.IdAdjuntoSeleccion)
                                    {
                                        AdjuntoFormulario adjuntoFormulario = new AdjuntoFormulario();
                                        adjuntoFormulario.IdFormulario = formulario.Data.IdFormulario;
                                        adjuntoFormulario.IdAdjunto = iAdjunto;
                                        adjuntoFormulario.IdUsuarioCreacion = entity.IdUsuarioCreacion;
                                        adjunto = await _formularioRepository.SaveAdjuntoFormulario(adjuntoFormulario);
                                        if (!adjunto.Success)
                                        {
                                            respuesta.Detail = "Error grabar los tipos de archivos que se quieren adjuntar...";
                                            respuesta.Success = false;
                                            return respuesta;
                                        }
                                    }
                                }
                                else
                                {
                                    //Por que no hubo nada para guardar
                                    adjunto.Success = true;
                                }

                                if (entity.Archivo != null)
                                {
                                    foreach (var iArchivo in entity.Archivo)
                                    {
                                        ArchivoAdjunto archivoAdjunto = new ArchivoAdjunto();
                                        string nuevoNombre = QuitarEspaciosAcentosEnes(iArchivo.FileName);
                                        //Ruta para subir el archivo                             
                                        var filePath = ruta + formulario.Data.IdFormulario + nuevoNombre;
                                        archivoAdjunto.IdFormulario = formulario.Data.IdFormulario;
                                        archivoAdjunto.Nombre = Path.GetFileName(nuevoNombre);
                                        archivoAdjunto.Ruta = filePath;
                                        archivoAdjunto.IdUsuarioCreacion = entity.IdUsuarioCreacion;
                                        archivo = await _formularioRepository.SaveArchivoAdjunto(archivoAdjunto);
                                        if (archivo.Success)
                                        {
                                            using (var stream = System.IO.File.Create(filePath))
                                            {
                                                await iArchivo.CopyToAsync(stream);
                                            }
                                        }
                                        else
                                        {
                                            respuesta.Detail = "Error al adjuntar los Archivos...";
                                            respuesta.Success = false;
                                            return respuesta;
                                        }
                                    }
                                }
                                else
                                {
                                    //Por que no hubo nada para guardar
                                    archivo.Success = true;
                                }

                                if (entity.IdConsideracionSeleccion != null && entity.IdConsideracionSeleccion.Count > 0)
                                {
                                    foreach (var iConsideracion in entity.IdConsideracionSeleccion)
                                    {
                                        FormularioConsideracion consideracionFormulario = new FormularioConsideracion();
                                        consideracionFormulario.IdFormulario = formulario.Data.IdFormulario;
                                        consideracionFormulario.IdConsideracion = iConsideracion;
                                        consideracionFormulario.IdUsuarioCreacion = entity.IdUsuarioCreacion;
                                        consideracion = await _formularioRepository.SaveFormularioConsideracion(consideracionFormulario);
                                        if (!consideracion.Success)
                                        {
                                            respuesta.Detail = "Error al adjuntar las Consideraciones...";
                                            respuesta.Success = false;
                                            return respuesta;
                                        }
                                    }
                                }
                                else
                                {
                                    //Porque no hubo nada para guardar
                                    consideracion.Success = true;
                                }

                                if (adjunto.Success && consideracion.Success && adjunto.Success)
                                {
                                    respuesta.Detail = "Se registro el Formulario...";
                                    respuesta.Title = formulario.Data.IdFormulario.ToString();
                                    tran2.Complete();
                                }
                            }
                        }
                    }
                    if (formulario.Success && adjunto.Success && consideracion.Success)
                    {
                        respuesta.Title = formulario.Data.IdFormulario.ToString();
                        respuesta.Success = true;
                    }
                }
                else
                {
                    respuesta.Success = false;
                    respuesta.Detail = "La Programacion ya tiene asignado un Formulario...";
                }
            }
            catch (Exception ex)
            {
                throw;
                respuesta.Success = false;
                respuesta.Title = "Sucedió un error al guardar el formulario. Actualice e intente de nuevo.";
                respuesta.Detail = ex.Message;
            }
            return respuesta;
        }

        public async Task<StatusReponse<Formulario>> FormularioPorId(int formulario)
        {
            StatusReponse<Formulario> status = null;
            status = await this.ComplexResponse(() => _formularioRepository.FormularioPorId(formulario));
            return status;
        }

        public async Task<StatusReponse<Formulario>> ObtenerPorId(int id)
        {
            StatusReponse<Formulario> status = null;

            if (id <= 0)
            {
                status = new StatusReponse<Formulario>(false, "Los datos ingresados para la consulta no son válidos");
                status.Errors = new Dictionary<string, List<string>>() { { "Id", new List<string>() { "El id es incorrecto" } } };
                return status;
            }

            status = await this.ComplexResponse(() => this._formularioRepository.ObtenerPorId(id), "Ok");
            return status;
        }

        private string QuitarEspaciosAcentosEnes(string cadena)
        {
            string cadenaNormalizada = cadena.Normalize(NormalizationForm.FormD);
            string cadenaSinTildesEnes = new String(cadenaNormalizada.
                                           Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).
                                           ToArray()).Normalize(NormalizationForm.FormC);
            string cadenaSinEspacios = cadenaSinTildesEnes.Replace(" ", String.Empty);
            return cadenaSinEspacios;
        }

        public async Task<StatusResponse> Update(Formulario entity, string ruta)
        {

            StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error al actualizar los datos del formulario. Actualice e intente de nuevo" };

            try
            {
                #region Hansel: Quitar por marcha blanca
                //if (entity.Archivo.Count < 1 || entity.IdAdjuntoSeleccion.Count < 1)
                //{
                //    respuesta.Detail = "Falta adjuntar o seleccionar los archivos...";
                //    respuesta.Success = false;
                //    return respuesta;
                //}
                #endregion

                #region Hansel: Por marcha blanca

                if (entity.Archivo != null && entity.Archivo.Count == 0 && (entity.IdAdjuntoSeleccion == null || entity.IdAdjuntoSeleccion.Count > 0))
                {
                    respuesta.Detail = "Usted ha indicado que cargará algunos tipos de documento, sin embargo no ha cargado ningún archivo";
                    respuesta.Success = false;
                    return respuesta;
                }

                if ((entity.Archivo == null || entity.Archivo.Count > 0) && entity.IdAdjuntoSeleccion != null && entity.IdAdjuntoSeleccion.Count == 0)
                {
                    respuesta.Detail = "Debe indicar que tipo de documentos son los que archivos que está cargando";
                    respuesta.Success = false;
                    return respuesta;
                }

                #endregion

                if (entity.IdUsuarioCreacion != entity.IdUsuarioEdicion)
                {
                    respuesta.Detail = "El formulario solo puede ser editado por el creador...";
                    respuesta.Success = false;
                    return respuesta;
                }
                StatusResponse validaFormulario = await _formularioRepository.ValidaFormularioEdicion(entity.IdFormulario);
                if (!validaFormulario.Success)
                {
                    respuesta.Detail = "No se puede editar porque esta fuera de fecha...";
                    respuesta.Success = false;
                    return respuesta;
                }

                using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Lista de los archivos antes del delete
                    List<ArchivoAdjunto> archivoOld = await _formularioRepository.ArchivoAdjuntoPorFormulario(entity.IdFormulario);
                    //Eliminando los registros en las tablas FormularioConsideracion y ArchivoAdjunto
                    StatusResponse archivoDelete = await _formularioRepository.DeleteArchivoAdjunto(entity.IdFormulario);
                    StatusResponse consideracionDelete = await _formularioRepository.DeleteFormularioConsideracion(entity.IdFormulario);
                    StatusResponse adjuntoDelete = await _formularioRepository.DeleteAdjuntoFormulario(entity.IdFormulario);
                    if (!adjuntoDelete.Success || !consideracionDelete.Success || !archivoDelete.Success)
                    {
                        respuesta.Detail = "Error al elminar los registros de consideraciones o archivos...";
                        respuesta.Success = false;
                        return respuesta;
                    }

                    //Editando Formulario
                    if (entity.EsTransportistaEncargado)
                    {
                        entity.IdTipoDocumentoEncargado = entity.IdTipoDocumentoTransportista;
                        entity.DocumentoEncargado = entity.DocumentoTransportista;
                        entity.NombreEncargado = entity.NombreTransportista;
                        entity.SctrEncargado = entity.SctrTransportista;
                    }
                    StatusResponse formularioUpdate = await _formularioRepository.Update(entity);
                    if (!formularioUpdate.Success)
                    {
                        respuesta.Success = false;
                        respuesta.Detail = "Error al editar los datos del formulario...";
                        return respuesta;
                    }

                    //Grabando en la tabla AdjuntoFormulario los checks seleccionados en formulario(documentos)
                    StatusReponse<AdjuntoFormulario> adjunto = new StatusReponse<AdjuntoFormulario>() { Success = false, Title = "" };
                    if (entity.IdAdjuntoSeleccion != null)
                    {
                        foreach (var iAdjunto in entity.IdAdjuntoSeleccion)
                        {
                            AdjuntoFormulario adjuntoFormulario = new AdjuntoFormulario();
                            adjuntoFormulario.IdFormulario = entity.IdFormulario;
                            adjuntoFormulario.IdAdjunto = iAdjunto;
                            adjuntoFormulario.IdUsuarioCreacion = entity.IdUsuarioCreacion;
                            adjunto = await _formularioRepository.SaveAdjuntoFormulario(adjuntoFormulario);
                            if (!adjunto.Success)
                            {
                                respuesta.Detail = "Error grabar los tipos de archivos que se quieren adjuntar...";
                                respuesta.Success = false;
                                return respuesta;
                            }
                        }
                    }
                    else
                    {
                        adjunto.Success = true;
                    }

                    //Grabando los archivos adjuntos en la tabla ArchivoAdjunto (se graba la ruta, nombre, etc)
                    StatusReponse<ArchivoAdjunto> archivo = new StatusReponse<ArchivoAdjunto>() { Success = false, Title = "" };
                    if (entity.Archivo != null)
                    {
                        foreach (var iArchivo in entity.Archivo)
                        {
                            ArchivoAdjunto archivoAdjunto = new ArchivoAdjunto();
                            string nuevoNombre = QuitarEspaciosAcentosEnes(iArchivo.FileName);
                            //Ruta para subir el archivo                             
                            var filePath = ruta + entity.IdFormulario + nuevoNombre;
                            archivoAdjunto.IdFormulario = entity.IdFormulario;
                            archivoAdjunto.Nombre = Path.GetFileName(nuevoNombre);
                            archivoAdjunto.Ruta = filePath;
                            archivoAdjunto.IdUsuarioCreacion = entity.IdUsuarioCreacion;
                            archivo = await _formularioRepository.SaveArchivoAdjunto(archivoAdjunto);
                            if (archivo.Success)
                            {
                                using (var stream = System.IO.File.Create(filePath))
                                {
                                    await iArchivo.CopyToAsync(stream);
                                }
                            }
                            else
                            {
                                respuesta.Detail = "Error al adjuntar los Archivos...";
                                respuesta.Success = false;
                                return respuesta;
                            }
                        }
                    }
                    else
                    {
                        archivo.Success = true;
                    }
                    //Grabando en la tabla FormularioConsideracion los checks seleccionados (casco, etc)
                    StatusReponse<FormularioConsideracion> consideracion = new StatusReponse<FormularioConsideracion>() { Success = false, Title = "" };
                    if (entity.IdConsideracionSeleccion != null && entity.IdConsideracionSeleccion.Count > 0)
                    {
                        foreach (var iConsideracion in entity.IdConsideracionSeleccion)
                        {
                            FormularioConsideracion consideracionFormulario = new FormularioConsideracion();
                            consideracionFormulario.IdFormulario = entity.IdFormulario;
                            consideracionFormulario.IdConsideracion = iConsideracion;
                            consideracionFormulario.IdUsuarioCreacion = entity.IdUsuarioCreacion;
                            consideracion = await _formularioRepository.SaveFormularioConsideracion(consideracionFormulario);
                            if (!consideracion.Success)
                            {
                                respuesta.Detail = "Error al adjuntar las Consideraciones...";
                                respuesta.Success = false;
                                return respuesta;
                            }
                        }
                    }
                    else
                    {
                        consideracion.Success = true;
                    }

                    respuesta.Detail = "Los datos se actualizaron correctamente...";
                    respuesta.Success = true;
                    foreach (var item in archivoOld)
                    {
                        File.Delete(item.Ruta);
                    }
                    tran1.Complete();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuesta;
        }
    }
}
