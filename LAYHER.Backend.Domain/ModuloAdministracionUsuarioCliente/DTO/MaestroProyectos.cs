using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO
{
    public class MaestroProyectos
    {
        public int IdZona { get; set; }
        public string NombreZona { get; set; }
        public string Afe { get; set; }
        public string CompanyOwner { get; set; }
        public string LocalName { get; set; }
        public string NombreCliente { get; set; }
        public int IdCliente { get; set; }
        public string CorreoCliente { get; set; }
        public int IdComercial { get; set; }
        public string NombreComercial { get; set; }
        public string IdContrato { get; set; }
        public string NombreContrato { get; set; }
    }

    public class ProyectoPerfilPersona
    {
        public int IdProyectoPerfilPersona { get; set; }
        public int IdPerfilPersona { get; set; }
        public string afe { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
