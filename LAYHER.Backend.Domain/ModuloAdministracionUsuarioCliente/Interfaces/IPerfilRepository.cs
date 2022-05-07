using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces
{
    public interface IPerfilRepository
    {
        Task<List<Perfil>> List(int id);
        Task<List<Perfil>> ListByDocumento(string documento);
        Task<List<PerfilPersona>> ListPerfilPersona(int id);
        Task<List<PerfilPermiso>> ListPerfilPermiso(int IdPerfil, int IdPermiso);
        Task<StatusReponse<PerfilPersona>> SavePerfilPersona(PerfilPersona entity);
        Task<StatusResponse> UpdatePerfilPersona(PerfilPersona entity);
    }
}
