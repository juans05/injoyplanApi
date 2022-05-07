using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces
{
    public interface IProgramacionUnidadRepository
    {
        Task<List<ProgramacionUnidad>> List(ProgramacionUnidad entity);
        Task<List<ProgramacionUnidad>> ListProgramacionesCliente(ProgramacionUnidad entity);
        Task<List<ProgramacionUnidad>> ListProgramacionesProveedor(ProgramacionUnidad entity);
        Task<ProgramacionUnidad> ProgramacionUnidadTiempoPorId(int programacion);
        Task<ProgramacionUnidad> ProgramacionUnidadPorId(int programacion);
        Task<List<Calendario>> ListCalendario(DateTime fecha, string sede);
        Task<List<Calendario>> ListCalendarioAgente(DateTime fecha, string sede);
        Task<List<ProgramacionUnidad>> ListProgramacionObservacion(DateTime fecha, string sede);
        Task<List<ProgramacionUnidad>> ListProgramacionObservacionAgente(DateTime fecha, string sede);
        Task<List<ProgramacionUnidad>> ListProgramacionObservacionDetalle(int programacion);
        Task<StatusReponse<ProgramacionUnidad>> Save(ProgramacionUnidad entity);
        Task<StatusReponse<List<ProgramacionUnidad>>> ValidaCruceProgramacion(int tipo, string almacen, DateTime fecha);
        Task<StatusResponse> Update(ProgramacionUnidad entity);
        Task<StatusResponse> UpdateEstado(int programacion, int estado, int usuario);
        Task<StatusResponse> Delete(int id);
        Task<StatusResponse> DisableEnableProgramacion(int programacion, bool estado);
        Task<StatusResponse> UpdateFormularioProgramacion(int programacion, int formulario);
        Task<StatusResponse> UpdateProgramacionObservacion(ProgramacionUnidad entity);
        Task<Almacen> AlmacenPorIdProgramacionUnidad(int programacionId);
    }
}
