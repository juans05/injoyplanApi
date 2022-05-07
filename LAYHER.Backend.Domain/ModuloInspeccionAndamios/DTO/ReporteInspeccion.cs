using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO
{
    public class ReporteInspeccion
    {
        public int InspeccionAndamio_id { get; set; }
        public int EstadoInspeccionAndamio_id { get; set; }
        public int Cliente_id { get; set; }
        public string Proyecto { get; set; }
        public string Direccion { get; set; }
        public string ZonaTrabajo { get; set; }
        public string Responsable { get; set; }
        public string Cargo { get; set; }
        public string MarcaAndamio { get; set; }
        public decimal CapacidadCarga { get; set; }
        public decimal MetrosCuadrados { get; set; }
        public string SobreCargaUso { get; set; }
        public string Observacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        
        public string NombreCliente { get; set; }
        public string Estado { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public string TipoAndamio_Nombres { get; set; }
        public int Activo { get; set; }


        public List<OutCumplimiento> ListaCumplimiento { get; set; }
        public List<RegistroFotografico> ListaRegistroFotografico { get; set; }

        public List<OutRegistroFotografico> Fotos { get; set; }
    }
}
