using System;
using System.Collections.Generic;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class ConfiguracionApp
    {
        public int ConfiguracionApp_id  { get; set; }
        public int Usuario_id  { get; set; }
        public bool ModoHistorico { get; set; }
        public int CantidadAnios  { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public bool Activo { get; set; }
        public string strMode { get; set; }
        public DateTime? Fecha { get; set; }
        public int IdUsuario { get; set; }
    }
}
