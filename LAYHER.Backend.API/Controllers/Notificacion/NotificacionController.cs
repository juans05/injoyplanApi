using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LAYHER.Backend.Domain.Notificacion.Domain;
using Microsoft.AspNetCore.Authorization;
using LAYHER.Backend.Application.Notificacion;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Domain.Notificacion.DTO;
using LAYHER.Backend.Shared;
using System.Security.Claims;

namespace LAYHER.Backend.API.Controllers.Notificacion
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {

        private readonly NotificacionApp _notificacionApp;

        public NotificacionController(NotificacionApp notificacionApp)
        {
            this._notificacionApp = notificacionApp;

        }

        [HttpPost("registrodispositivo")]
        public async Task<ActionResult> RegistrarDispositivo(InDispositivo dispositivo)
        {
            int usuarioId = Helpers.Utils.GetLoggedUserId(HttpContext.User.Identity);

            StatusResponse status = await this._notificacionApp.RegistrarDispositivo(dispositivo, usuarioId, usuarioId);
            if (!status.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, status);

            return Ok(status);
        }

        [HttpPost("usuario")]
        [AllowAnonymous]
        public async Task<ActionResult> EnviarNotificacionPorUsuario(NotificacionUsuarioDTO notificacion)
        {
            StatusResponse status = await this._notificacionApp.EnviarNotificacionPorUsuario(notificacion);
            if (!status.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, status);

            return Ok(status);
        }

        [HttpPost("topico")]
        [AllowAnonymous]
        public async Task<ActionResult> EnviarNotificacionPorTopico(NotificacionTopicoDTO notificacion)
        {
            StatusResponse status = await this._notificacionApp.EnviarNotificacionPorTopico(notificacion);
            if (!status.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, status);

            return Ok(status);
        }

        [HttpPost("email")]
        [AllowAnonymous]
        public async Task<ActionResult> EnviarCorreo([FromBody] InCorreo correo)
        {
            StatusResponse status = await this._notificacionApp.EnviarCorreo(correo.Asunto, correo.Cuerpo, correo.Destinatarios);
            if (!status.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, status);

            return Ok(status);
        }

        //private IHubContext<NotificacionesHub> _hubContext;

        //public NotificacionController(IHubContext<NotificacionesHub> hubContext)
        //{
        //    _hubContext = hubContext;
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public string Post([FromQuery] string mensaje)
        //{
        //    _hubContext.Clients.All.SendAsync("ReceiveMessage", new MensajeNotificacion() { Mensaje = mensaje });
        //    return "I have been called!";
        //}
    }
}
