using LAYHER.Backend.Domain.Notificacion.Domain;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.Notificacion.Interfaces
{
    public interface INotificacionProvider
    {
        Task<StatusResponse> EnviarMensajePorDispositivo(string tokenDispositivo, string titulo, string cuerpo, Dictionary<string, string> data);
        Task<StatusResponse> EnviarMensajePorTopico(string topico, string titulo, string cuerpo, Dictionary<string, string> data);
    }
}
