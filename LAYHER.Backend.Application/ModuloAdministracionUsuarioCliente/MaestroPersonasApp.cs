using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente
{
    public class MaestroPersonasApp : BaseApp<MaestroPersonasApp>
    {
        private IMaestroPersonasRepository _maestroPersonasRepository;
        public MaestroPersonasApp(IMaestroPersonasRepository maestroPersonasRepository,
                                  ILogger<BaseApp<MaestroPersonasApp>> logger) : base()
        {
            this._maestroPersonasRepository = maestroPersonasRepository;
        }
        public async Task<StatusReponse<List<MaestroPersonas>>> List(int id, int offset, int fetch)
        {
            return await this.ComplexResponse(() => _maestroPersonasRepository.List(id,offset,fetch));
        }
        public async Task<StatusReponse<List<MaestroPersonas>>> ListCliente()
        {
            return await this.ComplexResponse(() => _maestroPersonasRepository.ListCliente());
        }
        public async Task<StatusReponse<MaestroPersonas>> Save(MaestroPersonas entity)
        {
            StatusReponse<MaestroPersonas> response = new StatusReponse<MaestroPersonas>();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _maestroPersonasRepository.Save(entity);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        public async Task<StatusResponse> Update(MaestroPersonas entity)
        {
            StatusResponse response = new StatusResponse();
            response.Detail = "Operacion fallida...";

            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _maestroPersonasRepository.Update(entity);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
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
