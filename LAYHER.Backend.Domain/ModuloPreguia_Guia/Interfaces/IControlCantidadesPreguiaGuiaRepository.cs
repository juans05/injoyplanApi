using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using LAYHER.Backend.Domain.ModuloProductos.DTO;
using LAYHER.Backend.Domain.ModuloWebShop.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloPreguia_Guia.Interfaces
{
    public interface IControlCantidadesPreguiaGuiaRepository
    {
        Task<StatusReponse<Producto>> SaveControlcantidadesEcommerce(ControlCantidadesShop controlCantidades);
    }
}
