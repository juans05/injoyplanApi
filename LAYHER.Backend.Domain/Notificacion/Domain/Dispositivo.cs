using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.Notificacion.Domain
{
    public class Dispositivo
    {
        public int? Id { get; set; }
        public int Usuario_Id { get; set; }
        public string TokenDispositivo { get; set; }
        public EOrigenDispositivo OrigenDispositivo { get; set; }
        public bool Estado { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTimeOffset FechaCreacion { get; set; }
        public int? IdUsuarioEdicion { get; set; }
        public DateTimeOffset? FechaEdicion { get; set; }
    }
}
