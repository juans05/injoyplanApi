using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public string Clave { get; set; }
    }
}
