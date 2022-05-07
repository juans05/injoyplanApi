using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
    }
    public class PermisoPerfilPersona
    {
        public int IdPermisoPerfilPersona { get; set; }
        public int IdPerfilPersona { get; set; }
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public int IdPersona { get; set; }
        public string NombrePersona { get; set; }
        public int IdPermiso { get; set; }
        public string NombrePermiso { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
        public string Documento { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string Clave { get; set; }
        public string Estado { get; set; }
        //Proyecto
        public int IdProyectoPerfilPersona { get; set; }
        public string Afe { get; set; }
        public string CompanyOwner { get; set; }
        public string LocalName { get; set; }
        public List<Permiso> Permisos { get; set; }
        public List<MaestroProyectos> Proyectos { get; set; }
        public int IdAccountLogin { get; set; }
        public int IdUsuarioPersonaCreacion { get; set; }
    }
}
