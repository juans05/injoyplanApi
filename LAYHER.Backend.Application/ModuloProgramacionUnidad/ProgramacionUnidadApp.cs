using LAYHER.Backend.Application.Notificacion;
using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Domain.Notificacion.DTO;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloProgramacionUnidad
{
    public class ProgramacionUnidadApp : BaseApp<ProgramacionUnidadApp>
    {
        private IProgramacionUnidadRepository _programacionUnidadRepository;
        private IMaestroPersonasRepository _maestroPersonasRepository;
        private ITipoProgramacionRepository _tipoProgramacionRepository;
        private NotificacionApp _notifiacion;
        public ProgramacionUnidadApp(IProgramacionUnidadRepository programacionUnidadRepository,
                                     IMaestroPersonasRepository maestroPersonasRepository,
                                     ITipoProgramacionRepository tipoProgramacionRepository,
                                     NotificacionApp notificacion,
                                     ILogger<BaseApp<ProgramacionUnidadApp>> logger) : base()
        {
            this._programacionUnidadRepository = programacionUnidadRepository;
            this._maestroPersonasRepository = maestroPersonasRepository;
            this._tipoProgramacionRepository = tipoProgramacionRepository;
            this._notifiacion = notificacion;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> List(ProgramacionUnidad entity)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            if (string.IsNullOrEmpty(entity.IdProyecto))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el nro. del Proyecto.");
                return status;
            }
            if (string.IsNullOrEmpty(entity.IdAlmacen))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el Almacen.");
                return status;
            }
            if (entity.FechaInicio == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de inicio.");
                return status;
            }
            if (entity.FechaFin == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin.");
                return status;
            }
            status =  await this.ComplexResponse(() => _programacionUnidadRepository.List(entity));
            return status;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ValidaCruceProgramacion(int tipo, string almacen, DateTime fecha)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            if (tipo == 0)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el Tipo de Proyecto.");
                return status;
            }
            if (string.IsNullOrEmpty(almacen))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el Almacen.");
                return status;
            }
            if (fecha == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha");
                return status;
            }
            status = await _programacionUnidadRepository.ValidaCruceProgramacion(tipo,almacen,fecha);
            return status;
        }
        public Task<StatusReponse<Almacen>> AlmacenPorIdProgramacionUnidad(int programacionId)
        {
            return this.ComplexResponse(() => _programacionUnidadRepository.AlmacenPorIdProgramacionUnidad(programacionId), "");
        }
        public async Task<StatusReponse<ProgramacionUnidad>> ProgramacionUnidadPorId(int id)
        {
            StatusReponse<ProgramacionUnidad> status = null;
            if (id == 0)
            {
                status = new StatusReponse<ProgramacionUnidad>(false, "Debe especificar el Nro. de Programacion");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ProgramacionUnidadPorId(id));
            if (status.Data is null)
            {
                status = new StatusReponse<ProgramacionUnidad>(false, "No se encontro una Programacion con ese Nro.");
                return status;
            }
            return status;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ListProgramacionesCliente(ProgramacionUnidad entity)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            if (string.IsNullOrEmpty(entity.IdProyecto))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el nro. del Proyecto.");
                return status;
            }
            if (string.IsNullOrEmpty(entity.IdAlmacen))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el Almacen.");
                return status;
            }
            if (entity.FechaInicio == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de inicio.");
                return status;
            }
            if (entity.FechaFin == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin.");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListProgramacionesCliente(entity));
            return status;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ListProgramacionesProveedor(ProgramacionUnidad entity)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            if (string.IsNullOrEmpty(entity.IdProyecto))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el nro. del Proyecto.");
                return status;
            }
            if (string.IsNullOrEmpty(entity.IdAlmacen))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar el Almacen.");
                return status;
            }
            if (entity.FechaInicio == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de inicio.");
                return status;
            }
            if (entity.FechaFin == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin.");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListProgramacionesProveedor(entity));
            return status;
        }
        public async Task<StatusReponse<List<Calendario>>> ListCalendario(DateTime fecha, string sede)
        {
            StatusReponse<List<Calendario>> status = null;
            if (fecha == DateTime.MinValue)
            {
                status = new StatusReponse<List<Calendario>>(false, "Debe especificar una Fecha.");
                return status;
            }
            if (string.IsNullOrEmpty(sede))
            {
                status = new StatusReponse<List<Calendario>>(false, "Debe especificar una Sede.");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListCalendario(fecha,sede));
            return status;
        }
        public async Task<StatusReponse<List<Calendario>>> ListCalendarioAgente(DateTime fecha, string sede)
        {
            StatusReponse<List<Calendario>> status = null;
            if (fecha == DateTime.MinValue)
            {
                status = new StatusReponse<List<Calendario>>(false, "Debe especificar una Fecha.");
                return status;
            }
            if (string.IsNullOrEmpty(sede))
            {
                status = new StatusReponse<List<Calendario>>(false, "Debe especificar una Sede.");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListCalendarioAgente(fecha, sede));
            return status;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ListProgramacionObservacion(DateTime fecha, string sede)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            if (fecha == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha.");
                return status;
            }
            if (string.IsNullOrEmpty(sede))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Sede.");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListProgramacionObservacion(fecha, sede));
            return status;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ListProgramacionObservacionAgente(DateTime fecha, string sede)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            if (fecha == DateTime.MinValue)
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha.");
                return status;
            }
            if (string.IsNullOrEmpty(sede))
            {
                status = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Sede.");
                return status;
            }
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListProgramacionObservacionAgente(fecha, sede));
            return status;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ListProgramacionObservacionDetalle(int programacion)
        {
            StatusReponse<List<ProgramacionUnidad>> status = null;
            status = await this.ComplexResponse(() => _programacionUnidadRepository.ListProgramacionObservacionDetalle(programacion));
            return status;
        }
        public async Task<StatusResponse> Save(ProgramacionUnidad entity, string layherSuite)
        {
            StatusResponse response = new StatusResponse();

            try 
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (entity.FechaInicio == DateTime.MinValue)
                    {
                        response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de inicio.");
                        return response;
                    }
                    if (entity.FechaFin == DateTime.MinValue)
                    {
                        response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin.");
                        return response;
                    }
                    if (entity.IdTipoProgramacion == 1)
                    {
                        if (entity.IdEstado == 1)
                        {
                            if (entity.FechaRevision == DateTime.MinValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de revision.");
                                return response;
                            }
                            if (entity.RevisionHoraInicio == DateTime.MinValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar un Inicio para la revision.");
                                return response;
                            }
                            if (entity.RevisionHoraFin == DateTime.MinValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar un Fin para la revision.");
                                return response;
                            }
                        }
                        else
                        {
                            if (entity.FechaRevision == DateTime.MinValue)
                            {
                                entity.FechaRevision = DateTime.MaxValue;
                            }
                            if (entity.RevisionHoraInicio == DateTime.MinValue)
                            {
                                entity.RevisionHoraInicio = DateTime.MaxValue;
                            }
                            if (entity.RevisionHoraFin == DateTime.MinValue)
                            {
                                entity.RevisionHoraFin = DateTime.MaxValue;
                            }
                        }
                        if (entity.AlquilerFin == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin de alquiler.");
                            return response;
                        }
                        if (entity.FechaLlegada == DateTime.MinValue)
                        {
                            entity.FechaLlegada = DateTime.MaxValue;
                        }
                        if (entity.TurnoLlegada == DateTime.MinValue)
                        {
                            entity.TurnoLlegada = DateTime.MaxValue;
                        }
                    }
                    else if (entity.IdTipoProgramacion == 3)
                    {
                        if (entity.AlquilerFin == DateTime.MinValue)
                        {
                            entity.AlquilerFin = DateTime.MaxValue;
                        }
                        if (entity.FechaRevision == DateTime.MinValue)
                        {
                            entity.FechaRevision = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraInicio == DateTime.MinValue)
                        {
                            entity.RevisionHoraInicio = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraFin == DateTime.MinValue)
                        {
                            entity.RevisionHoraFin = DateTime.MaxValue;
                        }
                        if (entity.FechaLlegada == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Llegada.");
                            return response;
                        }
                        if (entity.TurnoLlegada == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar un Turno de Llegada.");
                            return response;
                        }
                    }
                    else
                    {
                        if (entity.AlquilerFin == DateTime.MinValue)
                        {
                            entity.AlquilerFin = DateTime.MaxValue;
                        }
                        if (entity.FechaRevision == DateTime.MinValue)
                        {
                            entity.FechaRevision = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraInicio == DateTime.MinValue)
                        {
                            entity.RevisionHoraInicio = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraFin == DateTime.MinValue)
                        {
                            entity.RevisionHoraFin = DateTime.MaxValue;
                        }
                        if (entity.FechaLlegada == DateTime.MinValue)
                        {
                            entity.FechaLlegada = DateTime.MaxValue;
                        }
                        if (entity.TurnoLlegada == DateTime.MinValue)
                        {
                            entity.TurnoLlegada = DateTime.MaxValue;
                        }
                    }

                    if (entity.IdTipoProgramacion != 3)
                    {
                        entity.IdAlmacenDestino = entity.IdAlmacen;
                    }
                    response = await _programacionUnidadRepository.Save(entity);

                    if (response.Success)
                    {
                        //StatusResponse statusCorreoCliente = new StatusResponse();
                        StatusResponse statusCorreo = new StatusResponse();

                        //Notificacion
                        if (entity.IdTipoProgramacion == 1 || entity.IdTipoProgramacion == 2)
                        {
                            StatusResponse statusNotificacion = new StatusResponse();
                            NotificacionUsuarioDTO notificacion = new NotificacionUsuarioDTO();

                            notificacion.IdUsuario = entity.IdCliente;
                            notificacion.Titulo = "Programacion Unidades";
                            notificacion.Cuerpo = "Se registro una nueva Programacion de Unidades";
                            Dictionary<string, string> dato = new Dictionary<string, string>();
                            dato.Add(entity.IdTipoProgramacion.ToString(), entity.IdUsuarioCreacion.ToString());
                            notificacion.Datos = dato;
                            statusNotificacion = await _notifiacion.EnviarNotificacionPorUsuario(notificacion);
                        }
                        //Correos
                        if (!string.IsNullOrEmpty(entity.Correo))
                        {
                            List<string> correos = new List<string>();
                            correos = entity.Correo.Split(',').ToList();
                            //Correo de CLiente
                            StatusReponse<List<TipoProgramacion>> tProgramacion = null;
                            //StatusReponse<List<MaestroPersonas>> mpCliente = null;
                            tProgramacion = await this.ComplexResponse(() => _tipoProgramacionRepository.List(entity.IdTipoProgramacion));
                            //mpCliente = await this.ComplexResponse(() => _maestroPersonasRepository.List(entity.IdCliente, 0, 1));

                            //List<string> correoCliente = new List<string>();
                            //correoCliente.Add(entity.CorreoCliente);
                            string asunto = "Programacion Unidad";
                            string tipoUsuario = (entity.IdTipoProgramacion == 1 || entity.IdTipoProgramacion == 2) ? "Cliente" : "Proveedor";
                            string contenido = "Estimado " + tipoUsuario +", <br/>" +
                                                      "Se procede a separar cita de " + tProgramacion.Data[0].Nombre + " para el dia " +
                                                      entity.FechaInicio.ToString("dd/MM/yyyy") + " a " + entity.FechaInicio.ToString("HH:mm tt") + " horas, <br/>" +
                                                      "la cantidad de " + entity.Tonelada + " TN. <br/>" +
                                                      "<br/>" +
                                                      "Quedamos atentos a su confirmacion considerando la fecha y <br/>" +
                                                      "hora programada. Se comparte el link de LayherSuite para que proceda a <br/>" +
                                                      "registrar la informacion solicitada para la recepcion. <br/>" +
                                                      "<br/>" +
                                                      "Link: "+ "<a href=" + layherSuite +">" + layherSuite + "</a>" +"<br/>" +
                                                      "<br/>" +
                                                      "USURARIO: RUC <br/>" +
                                                      "CONTRASEÑA: RUC(en caso no haya cambiado su clave con anterioridad) <br/>" +
                                                      "<br/>" +
                                                      "En el formulario encontrará los datos que se requieren para el ingreso y <br/>" +
                                                      "recomendaciones";
                            statusCorreo = await _notifiacion.EnviarCorreo(asunto, contenido, correos);
                            if (statusCorreo.Success)
                            {
                                response.Detail = "Operacion exitosa...";
                            }
                            else
                            {
                                response.Detail = "Error en el envío del correo...";
                                response.Success = false;
                            }
                        }
                        if (statusCorreo.Success)
                        {
                            tran.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> Delete(int programacion)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _programacionUnidadRepository.Delete(programacion);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> DisableEnableProgramacion(int programacion, bool estado)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _programacionUnidadRepository.DisableEnableProgramacion(programacion, estado);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> Update(ProgramacionUnidad entity)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (entity.FechaInicio == DateTime.MinValue)
                    {
                        response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de inicio.");
                        return response;
                    }
                    if (entity.FechaFin == DateTime.MinValue)
                    {
                        response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin.");
                        return response;
                    }
                    if (entity.IdTipoProgramacion == 1)
                    {
                        if (entity.IdEstado == 1)
                        {
                            if (entity.FechaRevision == DateTime.MinValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de revision.");
                                return response;
                            }
                            if (entity.RevisionHoraInicio == DateTime.MinValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar un Inicio para la revision.");
                                return response;
                            }
                            if (entity.RevisionHoraFin == DateTime.MinValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar un Fin para la revision.");
                                return response;
                            }
                        }
                        else 
                        {
                            if (entity.FechaRevision == DateTime.MinValue)
                            {
                                entity.FechaRevision = DateTime.MaxValue;
                            }
                            if (entity.RevisionHoraInicio == DateTime.MinValue)
                            {
                                entity.RevisionHoraInicio = DateTime.MaxValue;
                            }
                            if (entity.RevisionHoraFin == DateTime.MinValue)
                            {
                                entity.RevisionHoraFin = DateTime.MaxValue;
                            }
                        }
                        if (entity.AlquilerFin == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de fin de alquiler.");
                            return response;
                        }
                        if (entity.FechaLlegada == DateTime.MinValue)
                        {
                            entity.FechaLlegada = DateTime.MaxValue;
                        }
                        if (entity.TurnoLlegada == DateTime.MinValue)
                        {
                            entity.TurnoLlegada = DateTime.MaxValue;
                        }
                    }
                    else if (entity.IdTipoProgramacion == 3)
                    {
                        if (entity.AlquilerFin == DateTime.MinValue)
                        {
                            entity.AlquilerFin = DateTime.MaxValue;
                        }
                        if (entity.FechaRevision == DateTime.MinValue)
                        {
                            entity.FechaRevision = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraInicio == DateTime.MinValue)
                        {
                            entity.RevisionHoraInicio = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraFin == DateTime.MinValue)
                        {
                            entity.RevisionHoraFin = DateTime.MaxValue;
                        }
                        if (entity.FechaLlegada == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Llegada.");
                            return response;
                        }
                        if (entity.TurnoLlegada == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar un Turno de Llegada.");
                            return response;
                        }
                    }
                    else
                    {
                        if (entity.AlquilerFin == DateTime.MinValue)
                        {
                            entity.AlquilerFin = DateTime.MaxValue;
                        }
                        if (entity.FechaRevision == DateTime.MinValue)
                        {
                            entity.FechaRevision = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraInicio == DateTime.MinValue)
                        {
                            entity.RevisionHoraInicio = DateTime.MaxValue;
                        }
                        if (entity.RevisionHoraFin == DateTime.MinValue)
                        {
                            entity.RevisionHoraFin = DateTime.MaxValue;
                        }
                        if (entity.FechaLlegada == DateTime.MinValue)
                        {
                            entity.FechaLlegada = DateTime.MaxValue;
                        }
                        if (entity.TurnoLlegada == DateTime.MinValue)
                        {
                            entity.TurnoLlegada = DateTime.MaxValue;
                        }
                    }

                    response = await _programacionUnidadRepository.Update(entity);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> UpdateEstado(int programacion, int estado, int usuario)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _programacionUnidadRepository.UpdateEstado(programacion,estado,usuario);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> UpdateProgramacionObservacion(ProgramacionUnidad entity)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _programacionUnidadRepository.UpdateProgramacionObservacion(entity);
                    if (response.Success)
                    {
                        if (entity.IdTipoProgramacion == 1 || entity.IdTipoProgramacion == 2)
                        {
                            //Correo
                            StatusReponse<MaestroPersonas> responseCliente = new StatusReponse<MaestroPersonas>() { Success = false };
                            responseCliente = await this.ComplexResponse(() => _maestroPersonasRepository.DatosClienteByProgramacion(entity.IdProgramacionUnidad));                            
                            if (responseCliente.Data != null)
                            {
                                //Notificacion
                                StatusResponse statusNotificacion = new StatusResponse();
                                NotificacionUsuarioDTO notificacion = new NotificacionUsuarioDTO();

                                notificacion.IdUsuario = responseCliente.Data.Persona;
                                notificacion.Titulo = "Programacion Unidades";
                                notificacion.Cuerpo = "Se registro una Observacion";
                                Dictionary<string, string> dato = new Dictionary<string, string>();
                                dato.Add(entity.IdTipoProgramacion.ToString(), entity.IdUsuarioCreacion.ToString());
                                notificacion.Datos = dato;
                                statusNotificacion = await _notifiacion.EnviarNotificacionPorUsuario(notificacion);

                                //Correo
                                StatusResponse statusCorreo = new StatusResponse();
                                List<string> correos = new List<string>();
                                correos.Add(responseCliente.Data.Correo);
                                string asunto = "Programacion Unidad";
                                string contenido = "Se registro una observacion por parte del Agente";
                                statusCorreo = await this.ComplexResponse(() => _notifiacion.EnviarCorreo(asunto, contenido, correos));
                                if (statusCorreo.Success)
                                {
                                    response.Detail = "Operacion exitosa...";
                                    tran.Complete();
                                }
                                else
                                {
                                    response.Success = false;
                                }
                            }
                            else 
                            { 
                                response.Detail = "Operacion exitosa...";
                                tran.Complete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> ProgramacionesCorreo(ProgramacionUnidad entity, string layherSuite)
        {
            StatusResponse status = new StatusResponse();
            if (string.IsNullOrEmpty(entity.Correo))
            {
                status.Success = false;
                status.Detail = "No se regsitraron correos para esta Programacion de Unidades";
                return status;
            }
            else
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<string> correos = new List<string>();
                    correos = entity.Correo.Split(',').ToList();
                    string asunto = "Programacion de Unidad";
                    StatusReponse<List<TipoProgramacion>> tProgramacion = null;
                    tProgramacion = await this.ComplexResponse(() => _tipoProgramacionRepository.List(entity.IdTipoProgramacion));
                    string tipoUsuario = (entity.IdTipoProgramacion == 1 || entity.IdTipoProgramacion == 2) ? "Cliente" : "Proveedor";
                    string contenido = "Estimado " + tipoUsuario + ", <br/>" +
                          "Se procede a separar cita de " + tProgramacion.Data[0].Nombre + " para el dia " +
                          entity.FechaInicio.ToString("dd/MM/yyyy") + " a " + entity.FechaInicio.ToString("HH:mm tt") + " horas, <br/>" +
                          "la cantidad de " + entity.Tonelada + " TN. <br/>" +
                          "<br/>" +
                          "Quedamos atentos a su confirmacion considerando la fecha y <br/>" +
                          "hora programada. Se comparte el link de LayherSuite para que proceda a <br/>" +
                          "registrar la informacion solicitada para la recepcion. <br/>" +
                          "<br/>" +
                          "Link: " + "<a href=" + layherSuite + ">" + layherSuite + "</a>" + "<br/>" +
                          "<br/>" +
                          "USURARIO: RUC <br/>" +
                          "CONTRASEÑA: RUC(en caso no haya cambiado su clave con anterioridad) <br/>" +
                          "<br/>" +
                          "En el formulario encontrará los datos que se requieren para el ingreso y <br/>" +
                          "recomendaciones";
                    status = await _notifiacion.EnviarCorreo(asunto, contenido, correos);
                    if (status.Success)
                    {
                        status.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }

                }
                return status;
            }
        }
               
    }
}
