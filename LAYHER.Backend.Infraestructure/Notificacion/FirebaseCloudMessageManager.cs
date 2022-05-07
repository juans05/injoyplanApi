using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using LAYHER.Backend.Domain.Notificacion.Domain;
using LAYHER.Backend.Domain.Notificacion.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.Notificacion
{
    public class FirebaseCloudMessageManager : INotificacionProvider
    {
        public async Task<StatusResponse> EnviarMensajePorDispositivo(string tokenDispositivo, string titulo, string cuerpo, Dictionary<string, string> data)
        {
            if (data == null)
                data = new Dictionary<string, string>();

            Notification notificacion = new Notification
            {
                Title = titulo,
                Body = cuerpo
            };

            return await this.EnviarNotificacion(ENotificacionDestino.TokenDispositivo, tokenDispositivo, notificacion, data);
        }

        public async Task<StatusResponse> EnviarMensajePorTopico(string topico, string titulo, string cuerpo, Dictionary<string, string> data)
        {
            if (data == null)
                data = new Dictionary<string, string>();

            Notification notificacion = new Notification
            {
                Title = titulo,
                Body = cuerpo
            };

            return await this.EnviarNotificacion(ENotificacionDestino.Topico, topico, notificacion, data);
        }

        private async Task<StatusResponse> EnviarNotificacion(ENotificacionDestino notificacionDestino, string topicoOdispositivo, Notification notitifacion, Dictionary<string, string> data)
        {
            StatusResponse status = null;

            FirebaseApp defaultApp = null;
            try
            {
                defaultApp = FirebaseApp.DefaultInstance;
                if (defaultApp == null)
                {
                    defaultApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "google-fcm-key.json")),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                status = new StatusResponse(false, "Ocurrio un error grave al enviar notificación. Comunique al Administrador del sistema");
                return status;
            }


            Console.WriteLine(defaultApp.Name); // "[DEFAULT]"
            var message = new Message()
            {
                Data = data,
                Notification = notitifacion,
            };

            if (ENotificacionDestino.Topico == notificacionDestino)
                message.Topic = topicoOdispositivo;
            else if (ENotificacionDestino.TokenDispositivo == notificacionDestino)
                message.Token = topicoOdispositivo;

            try
            {
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAsync(message);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                status = new StatusResponse(false, "No se pudo enviar el mensaje");
                return status;
            }

            return new StatusResponse(true, "Se envió el mensaje de manera satisfactoria");
        }
    }
}
