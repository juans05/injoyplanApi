using LAYHER.Backend.Domain.ModuloFacturacion.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloFacturacion.Interfaces
{
    public interface IFacturacionRepository
    {
        Task<StatusReponse<VentaCabecera>> SaveVentaCabecera(VentaCabecera ventaCabecera);
        Task<StatusReponse<VentaDetalle>> SaveVentaDetalle(VentaDetalle ventaDetalles);
    }
}
