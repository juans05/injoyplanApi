using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.Comun.Interfaces
{
    public interface IZonaRepository
    {
        Task<List<Zona>> List(int id);
        Task<List<Zona>> ListDepartamento();
        Task<List<Zona>> ListProvincia(string departamento);
        Task<List<Zona>> ListDistrito(string provincia);
    }
}
