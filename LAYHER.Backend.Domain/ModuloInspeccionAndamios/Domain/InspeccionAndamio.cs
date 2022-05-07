using System;
using System.Collections.Generic;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class InspeccionAndamio
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
        public string SobreCargaUso { get; set; }
        public string Observacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public bool Activo { get; set; }

        public int interfaz { get; set; }
        public string strMode { get; set; }

        public string NombreCliente { get; set; }
        public string TipoAndamio_Nombres { get; set; }
        public string Estado { get; set; }
        public int IdUsuario { get; set; }//Id de Usuario Responsable
        public string NombreUsuario { get; set; }//Nombre de Usuario Responsable

        public List<CheckList> ListaCheckList { get; set; }
        public List<RegistroFotografico> ListaRegistroFotografico { get; set; }
        public List<RegistroFotografico> ListaRegistroFotograficoEliminado { get; set; }
    }
}
