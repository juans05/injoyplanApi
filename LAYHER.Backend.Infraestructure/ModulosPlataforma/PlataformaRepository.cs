using Dapper;
using LAYHER.Backend.Domain.ModuloPlataforma.DTO;
using LAYHER.Backend.Domain.ModuloPlataforma.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModulosPlataforma
{
    public class PlataformaRepository : IPlataformaRepository
    {
        protected readonly ICustomConnection mConnection;

        public PlataformaRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }



        public async Task<List<Plataforma>> ListarPlataforma()
        {
            List<Plataforma> entity = new List<Plataforma>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Plataforma>("SP_ListarPlataforma",
                    new
                    {
                       
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Plataforma>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<StatusReponse<Plataforma>> Save(Plataforma entity)
        {
            StatusReponse<Domain.ModuloPlataforma.DTO.Plataforma> status = new StatusReponse<Domain.ModuloPlataforma.DTO.Plataforma>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloPlataforma.DTO.Plataforma>("SP_GuardarPlataforma",
                    new
                    {
                        @nombrePlataforma = entity.nombrePlataforma,
                        @idusuario = entity.idUsuario,
                        @urlImagenPlataforma = entity.iconos

                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Domain.ModuloPlataforma.DTO.Plataforma)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo Save");
                }
            }
            return status;
        }
    }
}
