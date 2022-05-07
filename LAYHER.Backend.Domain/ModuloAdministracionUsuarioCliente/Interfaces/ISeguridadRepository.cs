using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
//using LAYHER.Backend.Domain.Seguridad;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces
{
    public interface ISeguridadRepository
    {
        Task<StatusReponse<UsuarioPersona>> ValidaUsuario(LoginRequest resquest);
        Task<StatusResponse> CambioContrasena(MaestroPersonas entity);
        Task<bool> ResetearContrasena(int idUsuario, string clave);
        Task<StatusReponse<UsuarioCliente>> ValidaUsuarioCliente(UsuarioCliente resquest);
        Task<StatusResponse> ValidaListaNegra(string token);
        Task<StatusReponse<ListaNegra>> SaveListaNegra(ListaNegra entity);
        Task<StatusReponse<UsuarioPersona>> ObtenerUsuarioInternoPorId(int idUsuario);
    }
}
