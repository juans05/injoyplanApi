using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloEventos.DTO
{
    public class Eventos_Categoria
    { 
        public int id { get; set; }
        public int id_Eventos { get; set; }
        public int id_categoria { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
