using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class Adjunto
    {
        public int IdAdjunto { get; set; }
        public string NombreAdjunto { get; set; }
        public string NombreDocumento { get; set; }
        public string Ruta { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class AdjuntoFormulario
    {
        public int IdAdjuntoFormulario { get; set; }
        public int IdFormulario { get; set; }
        public int IdAdjunto { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
    public class ArchivoAdjunto
    {
        public int IdArchivoAdjunto { get; set; }
        public int IdFormulario { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
