using Dapper;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloProgramacionUnidad
{
    public class ProgramacionTiempoRepository : IProgramacionTiempoRepository
    {
        protected readonly ICustomConnection mConnection;

        public ProgramacionTiempoRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<ProgramacionTiempo>> List(int programacionTiempo, int programacionUnidad)
        {
            List<ProgramacionTiempo> entity = new List<ProgramacionTiempo>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionTiempo>("[PU].[Usp_ListProgramacionTiempo]",
                    new
                    {
                        @ProgramacionTiempo = programacionTiempo,
                        @ProgramacionUnidad = programacionUnidad
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionTiempo>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<StatusReponse<ProgramacionTiempo>> Save(ProgramacionTiempo entity)
        {
            StatusReponse<ProgramacionTiempo> status = new StatusReponse<ProgramacionTiempo>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionTiempo>("[PU].[Usp_SaveProgramacionTiempo]",
                    new
                    {
                        @ProgramacionUnidad = entity.IdProgramacionUnidad,
                        @TipoMontacarga = entity.IdTipoMontacarga,
                        @DescargaInicio = entity.DescargaInicio,
                        @DescargaFin = entity.DescargaFin,
                        @FechaRevision = entity.FechaRevision,
                        @InicioRevision = entity.InicioRevision,
                        @FinRevision = entity.FinRevision,
                        @FechaRevision2 = entity.FechaRevision2,
                        @InicioRevision2 = entity.InicioRevision2,
                        @FinRevision2 = entity.FinRevision2,
                        @RevisionMayorUno = entity.RevisionMayorUno,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (ProgramacionTiempo)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
    }
}
