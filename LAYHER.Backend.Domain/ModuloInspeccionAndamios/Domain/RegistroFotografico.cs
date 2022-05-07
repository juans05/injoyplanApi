using System;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class RegistroFotografico
    {
        public int RegistroFotografico_id { get; set; }
        public int InspeccionAndamio_id { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string NombreOriginal { get; set; }
        public bool EsFoto { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public bool Activo { get; set; }
        public string strMode { get; set; }
        public string NombreArchivoTemporal { get; set; }
    }
}
