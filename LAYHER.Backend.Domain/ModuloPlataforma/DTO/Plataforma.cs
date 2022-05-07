using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloPlataforma.DTO
{
    public class Plataforma
    {
        public int id { get; set; }
        public string nombrePlataforma { get; set; }
        public int idUsuario { get; set; }
        public int estado { get; set; }
        public string iconos { get; set; }
        public DateTime created_at { get; set; }
        public DateTime deleted_at { get; set; }
    }
}
