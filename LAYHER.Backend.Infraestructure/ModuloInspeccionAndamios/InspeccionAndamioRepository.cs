using Dapper;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using LAYHER.Backend.Domain.Inspeccion.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Shared;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;

namespace LAYHER.Backend.Infraestructure.ModuloInspeccionAndamios
{
    public class InspeccionAndamioRepository : IInspeccionAndamioRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<InspeccionAndamioRepository> _logger;

        public InspeccionAndamioRepository(ICustomConnection _connection, ILogger<InspeccionAndamioRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }

        public async Task<List<InspeccionAndamio>> ListarInspeccionAndamio(
            string nombreProyecto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int offset,
            int fetch,
            bool? verHistorial,
            bool modoHistorico = false,
            int cantidadAnios = 3)
        {
            List<InspeccionAndamio> lista = new List<InspeccionAndamio>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<InspeccionAndamio>("[INSPECCION].[Usp_ListarInspeccionAndamio]",
                    new
                    {
                        @nombreProyecto = nombreProyecto,
                        @fechaInicio = fechaInicio,
                        @fechaFin = fechaFin,
                        @offset = offset,
                        @fetch = fetch,
                        @verHistorial = verHistorial,
                        @modoHistorico = modoHistorico,
                        @cantidadAnios = cantidadAnios,
                    }, commandType: CommandType.StoredProcedure);
                    lista = (List<InspeccionAndamio>)items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return lista;
        }

        public async Task<InspeccionAndamio> Obtener(int inspeccionAndamio_id, bool incluirCheckList = false, bool incluirRegistroFotografico = false)
        {
            InspeccionAndamio item = new InspeccionAndamio();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<InspeccionAndamio>("[INSPECCION].[Usp_CRUD_InspeccionAndamio]",
                    new
                    {
                        @strMode = "R",
                        @InspeccionAndamio_id = inspeccionAndamio_id
                    }, commandType: CommandType.StoredProcedure);
                    item = ((List<InspeccionAndamio>)items).ToList()[0];
                    if (incluirCheckList && item.InspeccionAndamio_id > 0)
                    {
                        IEnumerable<CheckList> itemsCheckLists = await scope.QueryAsync<CheckList>("[INSPECCION].[Usp_CRUD_CheckList]",
                        new
                        {
                            @strMode = "R",
                            @InspeccionAndamio_id = inspeccionAndamio_id
                        }, commandType: CommandType.StoredProcedure);


                        item.ListaCheckList = itemsCheckLists.ToList();
                    }
                    if (incluirRegistroFotografico)
                    {
                        IEnumerable<RegistroFotografico> itemsRegistroFotografico = await scope.QueryAsync<RegistroFotografico>("[INSPECCION].[Usp_CRUD_RegistroFotografico]",
                        new
                        {
                            @strMode = "R1",
                            @InspeccionAndamio_id = inspeccionAndamio_id
                        }, commandType: CommandType.StoredProcedure);

                        item.ListaRegistroFotografico = itemsRegistroFotografico.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return item;
        }

        public async Task<ReporteInspeccion> ObtenerParaReporte(int inspeccionAndamio_id)
        {
            ReporteInspeccion item = null;

            using (var scope = await mConnection.BeginConnection())
            {
                using (var gridReader = await scope.QueryMultipleAsync("INSPECCION.SPU_OBTENER_REPORTE", new { InspeccionAndamio_id = inspeccionAndamio_id }, commandType: CommandType.StoredProcedure))
                {
                    item = await gridReader.ReadFirstOrDefaultAsync<ReporteInspeccion>();
                    if (item != null)
                    {
                        //item.ListaCheckList = (await gridReader.ReadAsync<OutCheckList>()).ToList();
                        item.ListaCumplimiento = (await gridReader.ReadAsync<OutCumplimiento>()).ToList();
                        item.ListaRegistroFotografico = (await gridReader.ReadAsync<RegistroFotografico>()).ToList();
                    }
                }
            }
            return item;
        }

        public async Task<StatusReponse<InspeccionAndamio>> Registrar(InspeccionAndamio inspeccionAndamio)
        {
            StatusReponse<InspeccionAndamio> status = new StatusReponse<InspeccionAndamio>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    InspeccionAndamio item = await scope.QueryFirstOrDefaultAsync<InspeccionAndamio>("[INSPECCION].[Usp_CRUD_InspeccionAndamio]",
                    new
                    {
                        @strMode = inspeccionAndamio.strMode,
                        @InspeccionAndamio_id = inspeccionAndamio.InspeccionAndamio_id,
                        @EstadoInspeccionAndamio_id = inspeccionAndamio.EstadoInspeccionAndamio_id,
                        @Cliente_id = inspeccionAndamio.Cliente_id,
                        @Proyecto = inspeccionAndamio.Proyecto,
                        @Direccion = inspeccionAndamio.Direccion,
                        @ZonaTrabajo = inspeccionAndamio.ZonaTrabajo,
                        @Responsable = inspeccionAndamio.Responsable,
                        @Cargo = inspeccionAndamio.Cargo,
                        @MarcaAndamio = inspeccionAndamio.MarcaAndamio,
                        @SobreCargaUso = inspeccionAndamio.SobreCargaUso,
                        @Observacion = inspeccionAndamio.Observacion,
                        @IdUsuarioCreacion = inspeccionAndamio.IdUsuarioCreacion,
                        @IdUsuarioEdicion = inspeccionAndamio.IdUsuarioEdicion,
                        @FechaCreacion = inspeccionAndamio.FechaCreacion,
                        @FechaEdicion = inspeccionAndamio.FechaEdicion,
                        @Activo = inspeccionAndamio.Activo
                    }, commandType: CommandType.StoredProcedure);

                    if (inspeccionAndamio.strMode != "R")
                    {
                        item = await scope.QueryFirstOrDefaultAsync<InspeccionAndamio>("[INSPECCION].[Usp_CRUD_InspeccionAndamio]",
                                new
                                {
                                    @strMode = "R",
                                    @InspeccionAndamio_id = item.InspeccionAndamio_id
                                }, commandType: CommandType.StoredProcedure);
                    }
                    status.Data = item;
                    status.Success = true;
                }
                catch (Exception ex)
                {
                    throw;
                    status.Detail = ex.Message;
                }
            }
            return status;
        }

        public async Task<List<PreguntaInspeccion>> ListarPreguntaInspeccionAndamio(int tipoAndamio_id, int? subTipoAndamio_id)
        {
            List<PreguntaInspeccion> lista = new List<PreguntaInspeccion>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PreguntaInspeccion>("[INSPECCION].[Usp_ListarPreguntaInspeccionAndamio]",
                    new
                    {
                        @tipoAndamio_id = tipoAndamio_id,
                        @subTipoAndamio_id = subTipoAndamio_id,
                    }, commandType: CommandType.StoredProcedure);
                    lista = (List<PreguntaInspeccion>)items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return lista;
        }

        public async Task<List<SubTipoAndamio>> ListarSubTipoAndamios()
        {
            List<SubTipoAndamio> lista = new List<SubTipoAndamio>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<SubTipoAndamio>("[INSPECCION].[Usp_ListarPreguntaInspeccionAndamioSubTipoAndamio]",
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);
                    lista = (List<SubTipoAndamio>)items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return lista;
        }

        public async Task<List<TipoAndamio>> ListarTipoAndamio()
        {
            List<TipoAndamio> lista = new List<TipoAndamio>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<TipoAndamio>("[INSPECCION].[Usp_ListarTipoAndamio]",
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);
                    lista = (List<TipoAndamio>)items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return lista;
        }
        public async Task<List<Cliente>> ListarCliente(string documentoUsuario)
        {
            List<Cliente> lista = new List<Cliente>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Cliente>("[INSPECCION].[Usp_ListarCliente]",
                    new
                    {
                        @documentoUsuario = documentoUsuario,
                    }, commandType: CommandType.StoredProcedure);
                    lista = (List<Cliente>)items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return lista;
        }

        public async Task<StatusReponse<InspeccionAndamio>> ActualizarHistorico(InspeccionAndamio inspeccionAndamio)
        {
            StatusReponse<InspeccionAndamio> status = new StatusReponse<InspeccionAndamio>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<InspeccionAndamio>("[INSPECCION].[Usp_CRUD_InspeccionAndamio]",
                    new
                    {
                        @strMode = "U4",
                        @InspeccionAndamio_id = inspeccionAndamio.InspeccionAndamio_id,
                        @EstadoInspeccionAndamio_id = inspeccionAndamio.EstadoInspeccionAndamio_id,
                        @IdUsuarioEdicion = inspeccionAndamio.IdUsuarioEdicion,
                        @FechaEdicion = inspeccionAndamio.FechaEdicion,

                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (InspeccionAndamio)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception ex)
                {
                    throw;
                    status.Detail = ex.Message;
                }
            }
            return status;
        }
    }
}
