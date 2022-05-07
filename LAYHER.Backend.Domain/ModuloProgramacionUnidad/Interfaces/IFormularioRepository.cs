using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces
{
    public interface IFormularioRepository
    {
        Task<StatusReponse<Formulario>> Save(Formulario entity);
        Task<Formulario> FormularioPorId(int id);
        Task<List<ArchivoAdjunto>> ArchivoAdjuntoPorFormulario(int formulario);
        Task<StatusReponse<AdjuntoFormulario>> SaveAdjuntoFormulario(AdjuntoFormulario entity);
        Task<StatusReponse<ArchivoAdjunto>> SaveArchivoAdjunto(ArchivoAdjunto entity);
        Task<StatusReponse<FormularioConsideracion>> SaveFormularioConsideracion(FormularioConsideracion entity);
        Task<StatusResponse> Update(Formulario entity);
        Task<Formulario> ObtenerPorId(int id);
        Task<StatusResponse> DeleteFormulario(int id);
        Task<StatusResponse> ValidaFormularioEdicion(int id);
        Task<StatusResponse> DeleteArchivoAdjunto(int id);
        Task<StatusResponse> DeleteFormularioConsideracion(int id);
        Task<StatusResponse> DeleteAdjuntoFormulario(int id);
    }
}
