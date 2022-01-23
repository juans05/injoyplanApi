using System;
using System.Collections.Generic;
using System.Text;

namespace API.Domain.ModuloEventos.DTO
{
    public class Eventos_Fecha
    {
        public int id { get; set; }
        public int idEvento { get; set; }
        public string Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public double monto { get; set; }

        public string EventoLineaoPresencial { get; set; }

        public string cantvisita { get; set; }


        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }

    public class Evento_plataforma {
        public int id { get; set; }
        public int idEvento { get; set; }
        public int idPlataforma { get; set; }
        public string url { get; set; }

        public string NombrePlataforma { get; set; }
        public string estado { get; set; }
        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }

    public class Evento_Entradas { 
        public int id { get; set; }
        public int idEvento { get; set; }
        public int idEntrada { get; set; }

        public string NombrePlataforma { get; set; }
        public string url { get; set; }
        public string UrlWeb { get; set; }

        public string estado { get; set; }
        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
