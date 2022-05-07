using Dapper;
using LAYHER.Backend.Domain.Comun.DTO;
using LAYHER.Backend.Domain.Comun.Interfaces;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.Comun
{
    public class AreaAlmacenRepository : IAreaAlmacenRepository
    {
        protected readonly ICustomConnection mConnection;

        public AreaAlmacenRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<RegionAlmacen>> ListSede(int id)
        {
            List<RegionAlmacen> entity = new List<RegionAlmacen>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<RegionAlmacen>("[PU].[Usp_ListaSede]",
                    new
                    {
                        @AlmacenRegion = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<RegionAlmacen>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }

            return entity;
        }

        public async Task<List<AreaAlmacen>> ListAlmacen(int id)
        {
            List<AreaAlmacen> entity = new List<AreaAlmacen>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<AreaAlmacen>("[SEG].[USP_PERSONAALMACEN_LISTAR_X_PERSONA]",//[PU].[Usp_ListaAlmacen]
                    new
                    {
                        @IdPersona = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<AreaAlmacen>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<AreaAlmacen>> ListTodosAlmacenes()
        {
            List<AreaAlmacen> entity = new List<AreaAlmacen>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<AreaAlmacen>("[COMUN].[USP_LISTARALMACENES]",//[PU].[Usp_ListaAlmacen]
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<AreaAlmacen>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<Almacen> ObtenerAlmacenPorId(string idAlmacen)
        {
            Almacen almacen = null;

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    almacen = await scope.QueryFirstOrDefaultAsync<Almacen>("ALMACEN.USP_LG_MAESTROALMACEN_READ_BY_ID",
                    new
                    {
                        @AlmacenCodigo = idAlmacen
                    }, commandType: CommandType.StoredProcedure);

                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", e);
                }
            }
            return almacen;
        }
    }
}
