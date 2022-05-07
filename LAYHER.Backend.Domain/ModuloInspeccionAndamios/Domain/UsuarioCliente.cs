using System;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain
{
    public class UsuarioCliente
    {
        public int Usuario_id { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public int Cliente_id { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
