using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
    }
    public class PerfilPersona
    {
        public int IdPerfilPersona { get; set; }
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public int IdPersona { get; set; }
        public string NombrePersona { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
    }
    public class PerfilPermiso
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public int IdPermiso { get; set; }
        public string NombrePermiso { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
    }
}
