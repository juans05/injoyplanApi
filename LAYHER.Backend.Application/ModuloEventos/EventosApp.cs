using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloEventos;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Shared;
using System.Threading.Tasks;
using System.Transactions;
using LAYHER.Backend.Domain.ModuloEventos.DTO;
using LAYHER.Backend.Domain.ModuloEventos.Interfaces;

namespace LAYHER.Backend.Application.ModuloEventos
{
    public class EventosApp : BaseApp<EventosApp>
    {
        private IEventosRepository _eventoRepository;
        public EventosApp(IEventosRepository _EventoRepository,
                                   ILogger<BaseApp<EventosApp>> logger) : base()
        {
            this._eventoRepository = _EventoRepository;
        }

        public async Task<StatusReponse<Eventos>> Save(Eventos entity)
        {

            StatusReponse<Eventos> response = new StatusReponse<Eventos>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                response = await _eventoRepository.Save(entity);

                if (response.Success)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        public async Task<StatusResponse> SaveEventoAdjunto(EventoImagen entity)
        {

            StatusReponse<EventoImagen> response = new StatusReponse<EventoImagen>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                response = await _eventoRepository.SaveArchivoAdjunto(entity);

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

        public async Task<StatusReponse<List<ListarEventosAdmin>>> AdminList(int perfil, int usuario,string departamento,string provincia, string distrito, string titulo)
        {
            StatusReponse<List<ListarEventosAdmin>> status = null;
            status = await this.ComplexResponse(() => _eventoRepository.AdminListarEventos(perfil, usuario, departamento,  provincia,  distrito,  titulo));
            return status;
        }

        public async Task<StatusReponse<List<Eventos_Fecha>>> ListarEventoFecha(int id)
        {
            StatusReponse<List<Eventos_Fecha>> response = new StatusReponse<List<Eventos_Fecha>>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            response = await this.ComplexResponse(() => _eventoRepository.ConsultarEventoFecha(id));
            return response;
        }


        public async Task<StatusReponse<List<Eventos_Categoria>>> Listar_EventoCategoria(int id)
        {
            StatusReponse<List<Eventos_Categoria>> response = new StatusReponse<List<Eventos_Categoria>>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            response = await this.ComplexResponse(() => _eventoRepository.ConsultarEventoCategoria(id));
            return response;
        }


        public async Task<StatusReponse<List<EventoImagen>>> Listar_EventoImagen(int id)
        {
            StatusReponse<List<EventoImagen>> response = new StatusReponse<List<EventoImagen>>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            response = await this.ComplexResponse(() => _eventoRepository.ConsultarEventoImagen(id));
            return response;
        }

        public async Task<StatusReponse<List<Evento_plataforma>>> Listar_EventoPlataforma(int id)
        {
            StatusReponse<List<Evento_plataforma>> response = new StatusReponse<List<Evento_plataforma>>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            response = await this.ComplexResponse(() => _eventoRepository.ConsultarEventoPlataforma(id));
            return response;
        }
        public async Task<StatusReponse<List<Evento_Entradas>>> Listar_EventoEntradas(int id)
        {
            StatusReponse<List<Evento_Entradas>> response = new StatusReponse<List<Evento_Entradas>>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            response = await this.ComplexResponse(() => _eventoRepository.ConsultarEventoEntradas(id));
            return response;
        }
        public async Task<StatusReponse<Eventos>> editEventos(int id) {

            try
            {

                StatusReponse<Eventos> response = new StatusReponse<Eventos>() { Success = false, Title = "" };
                StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
                response = await this.ComplexResponse(() => _eventoRepository.EditarEvento(id));
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

       
        public async Task<StatusResponse> update(Eventos entity)
        {

            StatusReponse<Eventos> response = new StatusReponse<Eventos>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                response = await _eventoRepository.update(entity);

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
        public async Task<StatusResponse> delete(Eventos entity)
        {

            StatusReponse<Eventos> response = new StatusReponse<Eventos>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                response = await _eventoRepository.delete(entity.id);

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

        public async Task<StatusResponse> SaveEventoPlataforma(List<Evento_plataforma> entitys)
        {

            StatusReponse<Evento_plataforma> response = new StatusReponse<Evento_plataforma>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (entitys.Count >= 1)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var item in entitys)
                        {
                            response = await _eventoRepository.SaveEventoPlataforma(item);
                            if (!response.Success)
                            {
                                respuestaError.Detail = "Error grabar los eventos Plataforma.....";
                                respuestaError.Success = false;
                                return respuestaError;
                            }
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Plataforma registradas";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

                }
               

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
        public async Task<StatusResponse> SaveEventoFecha(List<Eventos_Fecha> entitys, bool tipo)
        {

            StatusReponse<Eventos_Fecha> response = new StatusReponse<Eventos_Fecha>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (entitys.Count >= 1)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var item in entitys)
                        {
                            response = await _eventoRepository.SaveEventoFecha(item, tipo);
                            if (!response.Success)
                            {
                                respuestaError.Detail = "Error grabar los eventos Fechas.....";
                                respuestaError.Success = false;
                                return respuestaError;
                            }
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Fechas registradadas";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

                }
               

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
        public async Task<StatusResponse> SaveEventoEntradas(List<Evento_Entradas> entitys)
        {

            StatusReponse<Evento_Entradas> response = new StatusReponse<Evento_Entradas>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (entitys.Count >= 1)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var item in entitys)
                        {
                            response = await _eventoRepository.SaveEventoEntradas(item);
                            if (!response.Success)
                            {
                                respuestaError.Detail = "Error grabar los eventos Entradas.....";
                                respuestaError.Success = false;
                                return respuestaError;
                            }
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Entradas registradadas";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

                }
               

               
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuestaError;
        }
        public async Task<StatusResponse> SaveEventoCategoria(List<Eventos_Categoria> entitys)
        {

            StatusReponse<Eventos_Categoria> response = new StatusReponse<Eventos_Categoria>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (entitys.Count >= 1)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var item in entitys)
                        {
                            response = await _eventoRepository.SaveEventoCategoria(item);
                            if (!response.Success)
                            {
                                respuestaError.Detail = "Error grabar los eventos Categoria.....";
                                respuestaError.Success = false;
                                return respuestaError;
                            }
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "eventos Categoria";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

                }



            }
            catch (Exception ex)
            {
                throw;
            }

            return respuestaError;
        }

        public async Task<StatusResponse> DeleteEventoCategoria(int id)
        {

            StatusReponse<Eventos_Categoria> response = new StatusReponse<Eventos_Categoria>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (id!=0)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        response = await _eventoRepository.DeleteEventoCategoria(id);
                        if (!response.Success)
                        {
                            respuestaError.Detail = "Error al Eliminar los Evento Categorias.....";
                            respuestaError.Success = false;
                            return respuestaError;
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Eliminar Evento Categoria";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }
                       
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuestaError;
        }
        public async Task<StatusResponse> DeleteEventoEntradas(int id)
        {

            StatusReponse<Evento_Entradas> response = new StatusReponse<Evento_Entradas>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (id != 0)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        response = await _eventoRepository.DeleteEventoEntradas(id);
                        if (!response.Success)
                        {
                            respuestaError.Detail = "Error al Eliminar los Evento Entradas.....";
                            respuestaError.Success = false;
                            return respuestaError;
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Eliminar Evento Entradas";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuestaError;
        }
        public async Task<StatusResponse> DeleteEventoPlataforma(int id)
        {

            StatusReponse<Evento_plataforma> response = new StatusReponse<Evento_plataforma>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (id != 0)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        response = await _eventoRepository.DeleteEventoPlataforma(id);
                        if (!response.Success)
                        {
                            respuestaError.Detail = "Error al Eliminar los Evento Entradas.....";
                            respuestaError.Success = false;
                            return respuestaError;
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Eliminar Evento Entradas";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

                }



               
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuestaError;
        }
        public async Task<StatusResponse> DeleteEventoFecha(int id)
        {

            StatusReponse<Eventos_Fecha> response = new StatusReponse<Eventos_Fecha>() { Success = false, Title = "" };
            StatusResponse respuestaError = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };
            try
            {
                if (id != 0)
                {
                    using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        response = await _eventoRepository.DeleteEventoFecha(id);
                        if (!response.Success)
                        {
                            respuestaError.Detail = "Error al Eliminar los Evento Entradas.....";
                            respuestaError.Success = false;
                            return respuestaError;
                        }
                        if (response.Success)
                        {
                            respuestaError.Detail = "Eliminar Evento Entradas";
                            respuestaError.Success = true;
                            tran1.Complete();
                        }

                    }

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
