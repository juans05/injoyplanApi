using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO
{
    public class LoginRequest
    {
        public int profile { get; set; }
        public string document { get; set; }
        public string password { get; set; }
    }
}
