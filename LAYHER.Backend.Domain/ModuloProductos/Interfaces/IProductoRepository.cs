using LAYHER.Backend.Domain.ModuloProductos.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloProductos.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ListarProductos();
    }
}
