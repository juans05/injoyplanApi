using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO
{

   
    public class MaestroPersonas
    {

        public int Persona { get; set; }
        public string vc_CodEmpresa { get; set; }
        public string TipoPersona { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }

        public string NombrePerfil { get; set; }
        public string NombreCompleto { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public string Busqueda { get; set; }

        public string Nombre { get; set; }

        public string EsProveedor { get; set; }

        public string DocumentoFiscal { get; set; }

        public string EsEmpleado { get; set; }
        public string EsCliente { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string NuevaClave { get; set; }
        public string Estado { get; set; }
        public int IdAccountLogin { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
    }
    public class UsuarioPersona
    {
        public int Persona { get; set; }
        public string NombreCompleto { get; set; }
        public string token { get; set; }
    }
    public class EmpresaEmpleado
    {
        public int IdEmpresaEmpleado { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEmpleado { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
        public int IdTipoRelacionEmpresaEmpleado { get; set; }
    }
}
