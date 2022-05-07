using LAYHER.Backend.Domain.Notificacion.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.Notificacion.DTO
{
    public class InDispositivo
    {
        public string TokenDispositivo { get; set; }
        public EOrigenDispositivo OrigenDispositivo { get; set; }
    }
}
