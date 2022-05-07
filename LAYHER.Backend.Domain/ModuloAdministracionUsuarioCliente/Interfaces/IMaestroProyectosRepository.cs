using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces
{
    public interface IMaestroProyectosRepository
    {
        Task<List<MaestroProyectos>> List(string id, int offset, int fetch);
        Task<List<MaestroProyectos>> ListProyectoCliente(string id);
        Task<List<MaestroProyectos>> ListByPersona(int persona, string proyectos, string afe, string localname, int zona, int offset, int fetch);
        Task<StatusReponse<ProyectoPerfilPersona>> SaveProyectoPerfilPersona(ProyectoPerfilPersona entity);
        Task<StatusResponse> DeleteProyectoPerfilPersona(int id);
        //Task<StatusReponse<MaestroProyectos>> UpdateProyectoStatus(string status, string afe);
        //Task<StatusReponse> UpdateProyectoPerfilPersona(ProyectoPerfilPersona entity);
    }
}
