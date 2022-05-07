using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloEventos.DTO
{
    public class ListarEventosAdmin {


        public int id { get; set; }
        public string titulo { get; set; }
        public string moneda { get; set; }
        public string distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string nombreCategoria { get; set; }
        public string nombreUsuario { get; set; }
    }
    public class ListarEventos {

        public int diaNum { get; set; }
        public int dia { get; set; }
        public int mes { get; set; }
        public DateTime fecha { get; set; }
        public int cantvisita { get; set; }
        public DateTime horaInicio { get; set; }
        public DateTime horaFin { get; set; }
        public int idFecha { get; set; }
        public int precio { get; set; }
        public int EventoLineaoPresencial { get; set; }

        public string nombrePlataforma { get; set; }
        public int estado { get; set; }

        public int destacado { get; set; }

        public int TituloLocal { get; set; }
        public string titulo { get; set; }

        public int Id { get; set; }
        public int url { get; set; }

        public int urlFuente { get; set; }

    }
    public class Eventos
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public int TipoEvento { get; set; }
        public string referencia { get; set; }
        public string direccion { get; set; }
        public string Numero { get; set; }
        public string latitud_longitud { get; set; }
        public string tipoMoneda { get; set; }
        public string descripcionEvento { get; set; }

        public int estado { get; set; }
        public string ubigeo { get; set; }
        public int Departamento { get; set; }
        public int Distrito { get; set; }
        public int Provincia { get; set; }
        public string enVivo { get; set; }

        public string TituloLocal { get; set; }
        public string DescripcionLocal { get; set; }
        public bool Destacado { get; set; }

        public string urlFuente { get; set; }

        public List<EventoImagen> EventoImagen { get; set; }
        public List<Eventos_Categoria> EventoCategoria { get; set; }
        public List<Eventos_Fecha> EventoFechas { get; set; }

        public List<Evento_plataforma> EventoPlataforma { get; set; }
        public List<Evento_Entradas> EventoEntradas { get; set; }

        public DateTime update_at { get; set; }

        public DateTime created_at { get; set; }

        public static explicit operator Eventos(string v)
        {
            throw new NotImplementedException();
        }
    }
    public class ListarEventoAdmin{
        public int perfil { get; set; }
        public int usuario { get; set; }
        public string distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string titulo { get; set; }
    }
    public class EventoImagen { 
        public string idEvento { get; set; }
        public IFormFile imagen { get; set; }
        public string url { get; set; }
        public string tipo { get; set; }
        public int principal { get; set; }
        public int consecutivoImg { get; set; }
    }
}
