using Dapper;
using LAYHER.Backend.Domain.Notificacion.Domain;
using LAYHER.Backend.Domain.Notificacion.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.Notificacion
{
    public class NotificacionRepository : INotificacionRepository
    {
        protected readonly ICustomConnection mConnection;

        public NotificacionRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Dispositivo>> ObtenerDispositivosDeUsuario(int idUsuario)
        {
            List<Dispositivo> lista = null;
            using (var scope = await mConnection.BeginConnection())
            {
                lista = (await scope.QueryAsync<Dispositivo>("NOTIFICACION.usp_obtener_dispositivos_por_usuario",
                new
                {
                    Usuario_Id = idUsuario,
                }, commandType: CommandType.StoredProcedure)).AsList();
            }

            return lista;
        }

        public async Task<StatusResponse> RegistrarDispositivo(Dispositivo item)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.ExecuteAsync("NOTIFICACION.usp_registrar_dispositivo",
                    new
                    {
                        Usuario_Id = item.Usuario_Id,
                        TokenDispositivo = item.TokenDispositivo,
                        OrigenDispositivo = item.OrigenDispositivo,
                        Estado = item.Estado,
                        IdUsuarioCreacion = item.IdUsuarioCreacion,
                        FechaCreacion = item.FechaCreacion,
                        IdUsuarioEdicion = item.IdUsuarioEdicion,
                        FechaEdicion = item.FechaEdicion,
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                }
                catch
                {
                    throw new CustomException("Sucedió un error al guardar datos del dispositivo");
                }
            }
            return status;
        }
    }
}
