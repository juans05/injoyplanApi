using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloWebShop.Interfaces
{
    public interface IWebShopRepository
    {
        Task<MaestroPersonas> ConsultaPersonaPorDNIoRUC(MaestroPersonas persona);
        Task<MaestroPersonas> ConsultaPersonaPorDNIoRUC(string tipoDocumentoPersonaEmpresa, string nroDocumentoPersonaEmpresa);

    }
}
