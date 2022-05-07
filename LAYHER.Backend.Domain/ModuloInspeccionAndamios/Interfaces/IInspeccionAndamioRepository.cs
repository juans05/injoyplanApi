using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Shared;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;

namespace LAYHER.Backend.Domain.Inspeccion.Interfaces
{
    public interface IInspeccionAndamioRepository
    {
        Task<List<InspeccionAndamio>> ListarInspeccionAndamio(
            string nombreProyecto, 
            DateTime? fechaInicio, 
            DateTime? fechaFin, 
            int offset, 
            int fetch,
            bool? verHistorial,
            bool modoHistorico = false,
            int cantidadAnios = 3);
        Task<StatusReponse<InspeccionAndamio>> Registrar(InspeccionAndamio checkList);
        Task<List<PreguntaInspeccion>> ListarPreguntaInspeccionAndamio(int tipoAndamio_id, int? subTipoAndamio_id);
        Task<List<SubTipoAndamio>> ListarSubTipoAndamios();
        Task<List<TipoAndamio>> ListarTipoAndamio();
        Task<List<Cliente>> ListarCliente(string documentoUsuario);
        Task<InspeccionAndamio> Obtener(int inspeccionAndamio_id, bool incluirCheckList = false, bool incluirRegistroFotografico = false);
        Task<ReporteInspeccion> ObtenerParaReporte(int inspeccionAndamio_id);
        Task<StatusReponse<InspeccionAndamio>> ActualizarHistorico(InspeccionAndamio inspeccionAndamio);
    }
}
