using Dapper;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Domain;
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
    public class ProgramacionUnidadRepository : IProgramacionUnidadRepository
    {
        protected readonly ICustomConnection mConnection;
        private IEspecificacionRepository _especificacionRepository;

        public ProgramacionUnidadRepository(ICustomConnection _connection, IEspecificacionRepository especificacionRepository)
        {
            this.mConnection = _connection;
            this._especificacionRepository = especificacionRepository;
        }

        public async Task<List<ProgramacionUnidad>> List(ProgramacionUnidad programacion)
        {
            List<ProgramacionUnidad> entity = new List<ProgramacionUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ListProgramacionUnidad]",
                    new
                    {
                        @ProgramacionUnidad = programacion.IdProgramacionUnidad,
                        @TipoProgramacion = programacion.IdTipoProgramacion,
                        @Proyecto = programacion.IdProyecto,
                        @FechaInicio = programacion.FechaInicio,
                        @FechaFin = programacion.FechaFin,
                        @Estado = programacion.IdEstado,
                        @Almacen = programacion.IdAlmacen,
                        @Persona = programacion.IdPersona
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionUnidad>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<StatusReponse<List<ProgramacionUnidad>>> ValidaCruceProgramacion(int tipo, string almacen, DateTime fecha)
        {
            StatusReponse<List<ProgramacionUnidad>> status = new StatusReponse<List<ProgramacionUnidad>> { Success = false, Detail = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ValidaCruceProgramacion]",
                    new
                    {
                        @TipoProgramacion = tipo,
                        @Almacen = almacen,
                        @Fecha = fecha
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (List<ProgramacionUnidad>)items;
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
        public async Task<List<ProgramacionUnidad>> ListProgramacionesCliente(ProgramacionUnidad programacion)
        {
            List<ProgramacionUnidad> entity = new List<ProgramacionUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ListBandejaCliente]",
                    new
                    {
                        @ProgramacionUnidad = programacion.IdProgramacionUnidad,
                        @TipoProgramacion = programacion.IdTipoProgramacion,
                        @Proyecto = programacion.IdProyecto,
                        @FechaInicio = programacion.FechaInicio,
                        @FechaFin = programacion.FechaFin,
                        @Estado = programacion.IdEstado,
                        @Almacen = programacion.IdAlmacen,
                        @Persona = programacion.IdPersona
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionUnidad>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<ProgramacionUnidad>> ListProgramacionesProveedor(ProgramacionUnidad programacion)
        {
            List<ProgramacionUnidad> entity = new List<ProgramacionUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ListBandejaProveedor]",
                    new
                    {
                        @ProgramacionUnidad = programacion.IdProgramacionUnidad,
                        @Proyecto = programacion.IdProyecto,
                        @FechaInicio = programacion.FechaInicio,
                        @FechaFin = programacion.FechaFin,
                        @Estado = programacion.IdEstado,
                        @Almacen = programacion.IdAlmacen,
                        @Persona = programacion.IdPersona
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionUnidad>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<ProgramacionUnidad> ProgramacionUnidadTiempoPorId(int programacion)
        {
            ProgramacionUnidad entity = new ProgramacionUnidad();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ProgramacionUnidadTiempoPorId]",
                    new
                    {
                        @ProgramacionUnidad = programacion
                    }, commandType: CommandType.StoredProcedure);
                    entity = items.FirstOrDefault();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<ProgramacionUnidad> ProgramacionUnidadPorId(int programacion)
        {
            ProgramacionUnidad entity = new ProgramacionUnidad();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    entity = await scope.QueryFirstOrDefaultAsync<ProgramacionUnidad>("[PU].[Usp_ProgramacionUnidadPorId]",
                    new
                    {
                        @ProgramacionUnidad = programacion
                    }, commandType: CommandType.StoredProcedure);

                    if (entity != null)
                    {
                        if (!string.IsNullOrEmpty(entity.Especificacion))
                        {
                            List<Especificacion> especificaciones = await this._especificacionRepository.List(0);
                            entity.NombreEspecificacion = especificaciones.Where(x => x.IdEspecificacion == int.Parse(entity.Especificacion)).First().Nombre;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<Calendario>> ListCalendario(DateTime fecha, string sede)
        {
            List<Calendario> entity = new List<Calendario>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Calendario>("[PU].[Usp_ListaCalendario]",
                    new
                    {
                        @Fecha = fecha,
                        @Sede = sede
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Calendario>)items;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<Calendario>> ListCalendarioAgente(DateTime fecha, string sede)
        {
            List<Calendario> entity = new List<Calendario>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Calendario>("[PU].[Usp_CalendarioAgente]",
                    new
                    {
                        @Fecha = fecha,
                        @Sede = sede
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Calendario>)items;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<ProgramacionUnidad>> ListProgramacionObservacion(DateTime fecha, string sede)
        {
            List<ProgramacionUnidad> entity = new List<ProgramacionUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ListProgramacionObservacion]",
                    new
                    {
                        @Fecha = fecha,
                        @Sede = sede
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionUnidad>)items;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<ProgramacionUnidad>> ListProgramacionObservacionAgente(DateTime fecha, string sede)
        {
            List<ProgramacionUnidad> entity = new List<ProgramacionUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ListProgramacionObservacionAgente]",
                    new
                    {
                        @Fecha = fecha,
                        @Sede = sede
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionUnidad>)items;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<ProgramacionUnidad>> ListProgramacionObservacionDetalle(int programacion)
        {
            List<ProgramacionUnidad> entity = new List<ProgramacionUnidad>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_ListaProgramacionObservacionDetalle]",
                    new
                    {
                        @ProgramacionUnidad = programacion
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ProgramacionUnidad>)items;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<StatusReponse<ProgramacionUnidad>> Save(ProgramacionUnidad entity)
        {
            StatusReponse<ProgramacionUnidad> status = new StatusReponse<ProgramacionUnidad>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProgramacionUnidad>("[PU].[Usp_SaveProgramacionUnidad]",
                    new
                    {
                        @TipoProgramacion = entity.IdTipoProgramacion,
                        @Proyecto = entity.IdProyecto,
                        @FechaInicio = entity.FechaInicio,
                        @FechaFin = entity.FechaFin,
                        @Estado = entity.IdEstado,
                        @TipoUnidad = entity.IdTipoUnidad,
                        @MetrajeTrailer = entity.IdMetrajeTrailer,
                        @Especificacion = entity.Especificacion,
                        @Almacen = entity.IdAlmacen,
                        @AlmacenDestino = entity.IdAlmacenDestino,
                        @Tonelada = entity.Tonelada,
                        @Correo = entity.Correo,
                        @FinAlquiler = entity.AlquilerFin,
                        @FechaRevision = entity.FechaRevision,
                        @RevisionHoraInicio = entity.RevisionHoraInicio,
                        @RevisionHoraFin = entity.RevisionHoraFin,
                        @Cotizacion = entity.IdCotizacion,
                        @NombreCliente = entity.NombreCliente,
                        @NombreProyecto = entity.NombreProyecto,
                        @FechaLlegada = entity.FechaLlegada,
                        @TurnoLlegada = entity.TurnoLlegada,
                        @TelefonoEncargado = entity.TelefonoEncargado,
                        @NumeroContenedores = entity.NumeroContenedores,
                        @Contenedor = entity.Contenedor,
                        @PackingList = entity.PackingList,
                        @TipoImportacion = entity.TipoImportacion,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (ProgramacionUnidad)items.FirstOrDefault();
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
        public async Task<StatusResponse> Update(ProgramacionUnidad entity)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[PU].[Usp_UpdateProgramacionUnidad]",
                    new
                    {
                        @ProgramacionUnidad = entity.IdProgramacionUnidad,
                        @FechaInicio = entity.FechaInicio,
                        @FechaFin = entity.FechaFin,
                        @Estado = entity.IdEstado,
                        @TipoUnidad = entity.IdTipoUnidad,
                        @MetrajeTrailer = entity.IdMetrajeTrailer,
                        @Especificacion = entity.Especificacion,
                        @Almacen = entity.IdAlmacen,
                        @Tonelada = entity.Tonelada,
                        @Correo = entity.Correo,
                        @FinAlquiler = entity.AlquilerFin,
                        @FechaRevision = entity.FechaRevision,
                        @RevisionHoraInicio = entity.RevisionHoraInicio,
                        @RevisionHoraFin = entity.RevisionHoraFin,
                        @Cotizacion = entity.IdCotizacion,
                        @NombreCliente = entity.NombreCliente,
                        @NombreProyecto = entity.NombreProyecto,
                        @FechaLlegada = entity.FechaLlegada,
                        @TurnoLlegada = entity.TurnoLlegada,
                        @TelefonoEncargado = entity.TelefonoEncargado,
                        @NumeroContenedores = entity.NumeroContenedores,
                        @Contenedor = entity.Contenedor,
                        @PackingList = entity.PackingList,
                        @TipoImportacion = entity.TipoImportacion,
                        @UsuarioEdicion = entity.IdUsuarioEdicion
                    }, commandType: CommandType.StoredProcedure);
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
        public async Task<StatusResponse> Delete(int id)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_EliminarPogramacionUnidad]",
                    new
                    {
                        @Programacion = id
                    }, commandType: CommandType.StoredProcedure);
                    var firstRow = items.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    response.Success = (bool)details[Heading[0]];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return response;
        }
        public async Task<StatusResponse> DisableEnableProgramacion(int programacion, bool estado)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_InhabilitarHabilitarProgramacion]",
                    new
                    {
                        @Programacion = programacion,
                        @Estado = estado
                    }, commandType: CommandType.StoredProcedure);
                    var firstRow = items.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    response.Success = (bool)details[Heading[0]];
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return response;
        }
        public async Task<StatusResponse> UpdateFormularioProgramacion(int programacion, int formulario)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_UpdateFormularioProgramacion]",
                    new
                    {
                        @ProgramacionUnidad = programacion,
                        @Formulario = formulario
                    }, commandType: CommandType.StoredProcedure);
                    var firstRow = items.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    response.Success = (bool)details[Heading[0]];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return response;
        }
        public async Task<StatusResponse> UpdateEstado(int programacion, int estado, int usuario)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[PU].[Usp_UpdateProgramacionEstado]",
                    new
                    {
                        @ProgramacionUnidad = programacion,
                        @Estado = estado,
                        @UsuarioEdicion = usuario
                    }, commandType: CommandType.StoredProcedure);
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
        public async Task<StatusResponse> UpdateProgramacionObservacion(ProgramacionUnidad entity)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[PU].[Usp_UpdateProgramacionObservacion]",
                    new
                    {
                        @ProgramacionUnidad = entity.IdProgramacionUnidad,
                        @Observacion = entity.Observacion,
                        @UnidadRecibida = entity.UnidadRecibida,
                        @Conforme = entity.Conforme,
                        @UsuarioEdicion = entity.IdUsuarioEdicion
                    }, commandType: CommandType.StoredProcedure);
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
        public async Task<Almacen> AlmacenPorIdProgramacionUnidad(int programacionId)
        {
            Almacen item = null;

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    item = await scope.QueryFirstOrDefaultAsync<Almacen>("[PU].[Usp_ObtenerAlmacenPorIdProgramacionUnidad]",
                    new
                    {
                        @IdProgramacionUnidad = programacionId
                    }, commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                    throw new CustomException("No se puede obtener el almacen de la programación con código " + programacionId, e);
                }
            }
            return item;
        }
    }
}
