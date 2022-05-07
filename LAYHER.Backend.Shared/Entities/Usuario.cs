using System;
using LAYHER.Backend.Shared;
using System.Threading.Tasks;

namespace LAYHER.Backend.Shared.Entities
{
    // [Table("Invoice")]
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Descripcion { get; set; }
        public string Clave { get; set; }
    }
}