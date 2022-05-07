using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Shared;

namespace LAYHER.Backend.Domain.Inspeccion.Interfaces
{
    public interface IRegistroFotograficoRepository
    {
        Task<StatusReponse<RegistroFotografico>> Registrar(RegistroFotografico registroFotografico);
        Task<List<RegistroFotografico>> Listar(int inspeccionAndamio_id);
        Task<StatusResponse> Eliminar(int registroFotografico_id);
        Task<RegistroFotografico> Obtener(string nombre);
    }
}