using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.Notificacion.DTO
{
    public class NotificacionUsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public Dictionary<string, string> Datos { get; set; }
    }
}
