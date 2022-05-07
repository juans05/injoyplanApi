using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NroDocumento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CorreoElectronico { get; set; }
        public string Rol { get; set; }
        public int Cliente_Id { get; set; }
        public string Cliente_NombreCompleto { get; set; }
    }
}
