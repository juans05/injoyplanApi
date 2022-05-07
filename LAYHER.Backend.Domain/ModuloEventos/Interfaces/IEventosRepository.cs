using LAYHER.Backend.Domain.ModuloEventos.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloEventos.Interfaces
{
    public interface  IEventosRepository
    {
        Task<StatusReponse<Eventos>> Save(Eventos entity);
        Task<StatusReponse<Eventos_Categoria>> SaveEventoCategoria(Eventos_Categoria entity);

        Task<StatusReponse<Evento_Entradas>> SaveEventoEntradas(Evento_Entradas entity);

        Task<StatusReponse<Evento_plataforma>> SaveEventoPlataforma(Evento_plataforma entity);     

        Task<StatusReponse<Eventos_Fecha>> SaveEventoFecha(Eventos_Fecha entity, bool tipo);

        Task<StatusReponse<Eventos>> update(Eventos entity);

        Task<StatusReponse<Eventos>> delete(int id);

        Task<List<Eventos>> listarEventos(Eventos entity);

        Task<StatusReponse<Eventos_Categoria>> DeleteEventoCategoria(int id);

        Task<StatusReponse<Evento_plataforma>> DeleteEventoPlataforma(int id);

        Task<StatusReponse<Evento_Entradas>> DeleteEventoEntradas(int id);

        Task<StatusReponse<Eventos_Fecha>> DeleteEventoFecha(int id);


        Task<StatusReponse<EventoImagen>> SaveArchivoAdjunto(EventoImagen entity);

        Task<List<ListarEventosAdmin>> AdminListarEventos(int perfil, int usuario, string departamento, string provincia, string distrito, string titulo);
        Task<Eventos> EditarEvento(int id);
        Task<List<Eventos_Categoria>> ConsultarEventoCategoria(int id);

        Task<List<Evento_plataforma>> ConsultarEventoPlataforma(int id);

        Task<List<Evento_Entradas>> ConsultarEventoEntradas(int id);

        Task<List<Eventos_Fecha>> ConsultarEventoFecha(int id);

        Task<List<EventoImagen>> ConsultarEventoImagen(int id);
    }
}
