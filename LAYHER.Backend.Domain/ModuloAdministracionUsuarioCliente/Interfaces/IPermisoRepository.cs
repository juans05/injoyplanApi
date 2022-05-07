using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces
{
    public interface IPermisoRepository
    {
        Task<List<Permiso>> List(int id);
        Task<List<PermisoPerfilPersona>> ListPermisoPerfilPersona(int id);
        Task<List<PermisoPerfilPersona>> ListPermisoProyectoPerfilPersona(int persona, int perfil, string afe, string documento, string nombre);
        Task<StatusReponse<PermisoPerfilPersona>> SavePermisoPerfilPersona(PermisoPerfilPersona entity);
        Task<StatusResponse> DeletePermisoPerfilPersona(int id);
        //Task<StatusReponse> UpdatePermisoPerfilPersona(PermisoPerfilPersona entity);       
    }
}
