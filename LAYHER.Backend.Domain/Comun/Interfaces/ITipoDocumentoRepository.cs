using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.Comun.Interfaces
{
    public interface ITipoDocumentoRepository
    {
        Task<List<TipoDocumento>> List(int id, int activo);
    }
}
