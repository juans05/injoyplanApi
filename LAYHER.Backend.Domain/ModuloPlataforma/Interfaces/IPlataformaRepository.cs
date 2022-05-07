using LAYHER.Backend.Domain.ModuloPlataforma.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.ModuloPlataforma.Interfaces
{
    public interface IPlataformaRepository
    {
        Task<List<Plataforma>> ListarPlataforma();
        Task<StatusReponse<Plataforma>> Save(Plataforma entity);
    }
}
