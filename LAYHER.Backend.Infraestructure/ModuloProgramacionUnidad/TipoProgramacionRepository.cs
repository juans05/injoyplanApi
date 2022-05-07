using Dapper;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloProgramacionUnidad
{
    public class TipoProgramacionRepository : ITipoProgramacionRepository
    {
        protected readonly ICustomConnection mConnection;

        public TipoProgramacionRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<TipoProgramacion>> List(int id)
        {
            List<TipoProgramacion> entity = new List<TipoProgramacion>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<TipoProgramacion>("[PU].[Usp_ListaTipoProgramacion]",
                    new
                    {
                        @Id = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<TipoProgramacion>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<ProgramacionEstado>> ListProgramacionEstado(int id)
        {
            List<ProgramacionEstado> entity = new List<ProgramacionEstado>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionEstado>("[PU].[Usp_ListaTipoProgramacionxEstado]",
                    new
                    {
                        @Id = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionEstado>)items;
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
