using LAYHER.Backend.Domain.Notificacion.Domain;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.Notificacion.Interfaces
{
    public interface INotificacionRepository
    {
        Task<StatusResponse> RegistrarDispositivo(Dispositivo dispositivo);
        Task<List<Dispositivo>> ObtenerDispositivosDeUsuario(int idUsuario);
    }
}
