using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces
{
    public interface IMaestroPersonasRepository
    {
        Task<List<MaestroPersonas>> List(int id, int offset, int fetch);
        Task<List<MaestroPersonas>> ListCliente();
        Task<MaestroPersonas> DatosClienteByProgramacion(int programacion);
        Task<StatusReponse<MaestroPersonas>> Save(MaestroPersonas entity);
        Task<StatusReponse<MaestroPersonas>> SaveLAyherShop(MaestroPersonas entity);
        
        Task<StatusReponse<EmpresaEmpleado>> SaveEmpresaEmpleado(EmpresaEmpleado entity);
        Task<StatusResponse> Update(MaestroPersonas entity);
        Task<StatusResponse> DisableEnablePersona(string documento, string estado);
        Task<StatusResponse> DeleteUsuario(string documento);
    }
}
