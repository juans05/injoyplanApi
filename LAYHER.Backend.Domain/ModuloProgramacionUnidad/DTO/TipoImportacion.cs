using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class TipoImportacion
    {
        public int IdTipoImportacion { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}
