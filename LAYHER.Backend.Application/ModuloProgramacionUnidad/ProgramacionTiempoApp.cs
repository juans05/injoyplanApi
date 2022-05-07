using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloProgramacionUnidad
{
    public class ProgramacionTiempoApp : BaseApp<ProgramacionTiempoApp>
    {
        private IProgramacionTiempoRepository _programacionTiempoRepository;
        private IProgramacionUnidadRepository _programacionUnidadRepository;
        public ProgramacionTiempoApp(IProgramacionTiempoRepository programacionTiempoRepository,
                                     IProgramacionUnidadRepository programacionUnidadRepository,
                                     ILogger<BaseApp<ProgramacionTiempoApp>> logger) : base()
        {
            this._programacionTiempoRepository = programacionTiempoRepository;
            this._programacionUnidadRepository = programacionUnidadRepository;
        }
        public async Task<StatusReponse<List<ProgramacionTiempo>>> List(int programacionTiempo, int programacionUnidad)
        {
            StatusReponse<List<ProgramacionTiempo>> status = null;
            status = await this.ComplexResponse(() => _programacionTiempoRepository.List(programacionTiempo,programacionUnidad));
            return status;
        }

        public async Task<StatusResponse> Save(ProgramacionTiempo entity)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                ProgramacionUnidad validaProgramacion = await _programacionUnidadRepository.ProgramacionUnidadTiempoPorId(entity.IdProgramacionUnidad);
                if (validaProgramacion.IdProgramacionTiempo == 0)
                {
                    using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        if (entity.DescargaInicio == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Inicio para la Descarga.");
                            return response;
                        }
                        if (entity.DescargaFin == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Fin para la Descarga.");
                            return response;
                        }
                        if (entity.FechaRevision == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha para la Revision.");
                            return response;
                        }
                        if (entity.InicioRevision == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Inicio para la Revision.");
                            return response;
                        }
                        if (entity.FinRevision == DateTime.MinValue)
                        {
                            response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Fin para la Revision.");
                            return response;
                        }
                        if (entity.RevisionMayorUno)
                        {
                            if (entity.FechaRevision2 == DateTime.MaxValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha para la Revision.");
                                return response;
                            }
                            if (entity.InicioRevision2 == DateTime.MaxValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Inicio para la Revision.");
                                return response;
                            }
                            if (entity.FinRevision2 == DateTime.MaxValue)
                            {
                                response = new StatusReponse<List<ProgramacionUnidad>>(false, "Debe especificar una Fecha de Fin para la Revision.");
                                return response;
                            }
                        }
                        response = await _programacionTiempoRepository.Save(entity);
                        if (response.Success)
                        {
                            response.Detail = "Operacion exitosa...";
                            tran.Complete();
                        }
                    }
                }
                else
                {
                    response.Success = false;
                    response.Detail = "La Programacion ya tiene asignado Programaciones de Tiempo...";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
