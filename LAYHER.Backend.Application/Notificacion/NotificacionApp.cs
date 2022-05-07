using FluentValidation.Results;
using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.Comun.Utils;
using LAYHER.Backend.Domain.Notificacion.Domain;
using LAYHER.Backend.Domain.Notificacion.DTO;
using LAYHER.Backend.Domain.Notificacion.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.Notificacion
{
    public class NotificacionApp : BaseApp<NotificacionApp>
    {
        private INotificacionProvider _notificacionProvider;
        private INotificacionRepository _notificacionRepository;
        private IMailManager _mailManager;

        public NotificacionApp(
            INotificacionProvider notificacionProvider,
            INotificacionRepository notificacionRepository,
            IMailManager mailManager,
            ILogger<BaseApp<NotificacionApp>> logger) : base()
        {
            this._notificacionProvider = notificacionProvider;
            this._notificacionRepository = notificacionRepository;
            this._mailManager = mailManager;
        }

        public async Task<StatusResponse> EnviarNotificacionPorUsuario(NotificacionUsuarioDTO notificacion)
        {
            StatusResponse status = new StatusResponse(true, "");
            try
            {
                //recuperamos todos los token dónde inició sesión el usuario
                List<Dispositivo> listaDispositivos = await this._notificacionRepository.ObtenerDispositivosDeUsuario(notificacion.IdUsuario);

                //enviamos la notificación a cada dispositivo
                foreach (Dispositivo dispositivo in listaDispositivos)
                {
                    await this._notificacionProvider.EnviarMensajePorDispositivo(dispositivo.TokenDispositivo, notificacion.Titulo, notificacion.Cuerpo, notificacion.Datos);
                }

                status.Title = listaDispositivos.Count == 0 ? "El usuario no tiene dispositivos subscritos para enviarle la notificación" : "Se envió la notificación al usuario";
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "IdUsuario: {0}", notificacion.IdUsuario);
                status.Success = false;
                status.Title = "Sucedió un erro al notificar al usuario";
            }
            return status;
        }

        public async Task<StatusResponse> EnviarNotificacionPorTopico(NotificacionTopicoDTO notificacion)
        {
            return await this._notificacionProvider.EnviarMensajePorTopico(notificacion.Topico, notificacion.Titulo, notificacion.Cuerpo, notificacion.Datos);
        }

        public async Task<StatusResponse> RegistrarDispositivo(InDispositivo dispositivo, int usuarioIdDispositivo, int usuarioIdCreador)
        {
            Dispositivo item = new Dispositivo();
            item.Usuario_Id = usuarioIdDispositivo;
            item.TokenDispositivo = dispositivo.TokenDispositivo;
            item.OrigenDispositivo = dispositivo.OrigenDispositivo;
            item.Estado = true;
            item.IdUsuarioCreacion = usuarioIdCreador;
            item.FechaCreacion = DateTime.UtcNow;

            item.IdUsuarioEdicion = item.IdUsuarioCreacion;
            item.FechaEdicion = item.FechaCreacion;

            return await MessageResponse(() => this._notificacionRepository.RegistrarDispositivo(item), "Se registró el dispositivo satisfactoriamente");
        }

        public async Task<StatusResponse> EnviarCorreo(string asunto, string contenido, List<string> destinatarios, bool esHTML = true, List<string> destinatariosCC = null, List<string> destinatariosCCO = null, List<Attachment> archivos = null)
        {
            InCorreo correoBasico = new InCorreo() { Asunto = asunto, Cuerpo = contenido, Destinatarios = destinatarios };
            InCorreoValidator validator = new InCorreoValidator();
            ValidationResult result = validator.Validate(correoBasico);
            if (!result.IsValid)
                return new StatusResponse("Los datos ingresados no son válidos", FluentValidationExtend.GetErrors(result.Errors));

            return await this._mailManager.SendEmail(asunto, contenido, destinatarios, destinatariosCC, destinatariosCCO, archivos, esHTML);
        }


    }
}
