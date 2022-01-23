
using API.Domain.ModuloComun.DTO;
using API.Domain.ModuloComun.Interfaces;
using API.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace API.Infraestructure.Comun
{
    public class ZonaRepository : IZonaRepository
    {
        protected readonly ICustomConnection mConnection;

        public ZonaRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Zona>> List(int id)
        {
            List<Zona> entity = new List<Zona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Zona>("SP_ListarDepartamento",
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Zona>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<Zona>> ListDepartamento()
        {
            List<Zona> entity = new List<Zona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Zona>("SP_ListarDepartamento",
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Zona>)items;
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<Zona>> ListProvincia(string departamento)
        {
            List<Zona> entity = new List<Zona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Zona>("SP_ListarProvincia",
                    new
                    {
                        @departamento = departamento
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Zona>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<Zona>> ListDistrito(string provincia)
        {
            List<Zona> entity = new List<Zona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Zona>("SP_ListarDistrito",
                    new
                    {
                        @Provincia = provincia
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Zona>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

   
    }
}
