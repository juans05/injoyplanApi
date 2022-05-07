using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloPlataforma.DTO;
using LAYHER.Backend.Domain.ModuloPlataforma.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Application.ModuloPlataforma
{
    public  class PlataformaApp : BaseApp<PlataformaApp>
    {
        private IPlataformaRepository _iplataformaRepository;
        public PlataformaApp(IPlataformaRepository _IPlataformaRepository,
                                   ILogger<BaseApp<PlataformaApp>> logger) : base()
        {
            this._iplataformaRepository = _IPlataformaRepository;
        }
        public async Task<StatusReponse<List<Plataforma>>> List()
        {
            StatusReponse<List<Plataforma>> status = null;
            status = await this.ComplexResponse(() => _iplataformaRepository.ListarPlataforma());
            return status;
        }
        public async Task<StatusResponse> Save(Plataforma entity)
        {

            StatusReponse<Plataforma> response = new StatusReponse<Plataforma>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                response = await _iplataformaRepository.Save(entity);

                if (response.Success)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuestaError;
        }
    }
}
