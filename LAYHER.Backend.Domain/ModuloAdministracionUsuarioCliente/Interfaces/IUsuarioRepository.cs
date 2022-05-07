using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerUsuarioPorNroDocumento(string nroDocumento);
        Task<Empresa> ObtenerEmpresaPorNroDocumento(string nroDocumento);
    }
}
