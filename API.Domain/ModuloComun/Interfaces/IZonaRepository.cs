
using API.Domain.ModuloComun.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.ModuloComun.Interfaces
{ 
    public interface IZonaRepository
    {
        Task<List<Zona>> List(int id);
        Task<List<Zona>> ListDepartamento();
        Task<List<Zona>> ListProvincia(string departamento);
        Task<List<Zona>> ListDistrito(string provincia);
    }
}
