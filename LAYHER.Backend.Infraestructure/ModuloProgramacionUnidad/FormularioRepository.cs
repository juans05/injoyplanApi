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
    public class FormularioRepository : IFormularioRepository
    {
        protected readonly ICustomConnection mConnection;

        public FormularioRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<StatusReponse<Formulario>> Save(Formulario entity)
        {
            StatusReponse<Formulario> status = new StatusReponse<Formulario>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Formulario>("[PU].[Usp_SaveFormulario]",
                    new
                    {
                        @ProgramacionUnidad = entity.ProgramacionUnidad,
                        @EsTransporteNacional = entity.EsTransporteNacional,
                        @EsTransportistaEncargado = entity.EsTransportistaEncargado,
                        @NumeroDocumento = entity.NumeroDocumento,
                        @RazonSocial = entity.RazonSocial,
                        @ModeloVehiculo = entity.ModeloVehiculo,
                        @PlacaTracto = entity.PlacaTracto,
                        @PlacaCarreta = entity.PlacaCarreta,
                        @TelefonoContacto = entity.TelefonoContacto,
                        @IdTipoDocumentoTransportista = entity.IdTipoDocumentoTransportista,
                        @DocumentoTransportista = entity.DocumentoTransportista,
                        @NombreTransportista = entity.NombreTransportista,
                        @SctrTransportista = entity.SctrTransportista,
                        @TelefonoTransportista = entity.TelefonoTransportista,
                        @LicenciaTransportista = entity.LicenciaTransportista,
                        @IdTipoDocumentoEncargado = entity.IdTipoDocumentoEncargado,
                        @DocumentoEncargado = entity.DocumentoEncargado,
                        @NombreEncargado = entity.NombreEncargado,
                        @SctrEncargado = entity.SctrEncargado,
                        @Consideracion = entity.Consideracion,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Formulario)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<Formulario> FormularioPorId(int formulario)
        {
            Formulario entity = new Formulario();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Formulario>("[PU].[Usp_ListaFormulario]",
                    new
                    {
                        @Formulario = formulario
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
        public async Task<List<ArchivoAdjunto>> ArchivoAdjuntoPorFormulario(int formulario)
        {
            List<ArchivoAdjunto> entity = new List<ArchivoAdjunto>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ArchivoAdjunto>("[PU].[Usp_ListaArchivoAdjunto]",
                    new
                    {
                        @Formulario = formulario
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ArchivoAdjunto>)items;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<StatusReponse<AdjuntoFormulario>> SaveAdjuntoFormulario(AdjuntoFormulario entity)
        {
            StatusReponse<AdjuntoFormulario> status = new StatusReponse<AdjuntoFormulario>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<AdjuntoFormulario>("[PU].[Usp_SaveAdjuntoFormulario]",
                    new
                    {
                        @Formulario = entity.IdFormulario,
                        @Adjunto = entity.IdAdjunto,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (AdjuntoFormulario)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<StatusReponse<ArchivoAdjunto>> SaveArchivoAdjunto(ArchivoAdjunto entity)
        {
            StatusReponse<ArchivoAdjunto> status = new StatusReponse<ArchivoAdjunto>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ArchivoAdjunto>("[PU].[Usp_SaveArchivoAdjunto]",
                    new
                    {
                        @Formulario = entity.IdFormulario,
                        @Nombre = entity.Nombre,
                        @Ruta = entity.Ruta,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (ArchivoAdjunto)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<StatusReponse<FormularioConsideracion>> SaveFormularioConsideracion(FormularioConsideracion entity)
        {
            StatusReponse<FormularioConsideracion> status = new StatusReponse<FormularioConsideracion>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<FormularioConsideracion>("[PU].[Usp_SaveFormularioConsideracion]",
                    new
                    {
                        @Formulario = entity.IdFormulario,
                        @Consideracion = entity.IdConsideracion,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (FormularioConsideracion)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<StatusResponse> Update(Formulario entity)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[PU].[Usp_UpdateFormulario]",
                    new
                    {
                        @Formulario = entity.IdFormulario,
                        @EsTransporteNacional = entity.EsTransporteNacional,
                        @EsTransportistaEncargado = entity.EsTransportistaEncargado,
                        @NumeroDocumento = entity.NumeroDocumento,
                        @RazonSocial = entity.RazonSocial,
                        @ModeloVehiculo = entity.ModeloVehiculo,
                        @PlacaTracto = entity.PlacaTracto,
                        @PlacaCarreta = entity.PlacaCarreta,
                        @TelefonoContacto = entity.TelefonoContacto,
                        @IdTipoDocumentoTransportista = entity.IdTipoDocumentoTransportista,
                        @DocumentoTransportista = entity.DocumentoTransportista,
                        @NombreTransportista = entity.NombreTransportista,
                        @SctrTransportista = entity.SctrTransportista,
                        @TelefonoTransportista = entity.TelefonoTransportista,
                        @LicenciaTransportista = entity.LicenciaTransportista,
                        @IdTipoDocumentoEncargado = entity.IdTipoDocumentoEncargado,
                        @DocumentoEncargado = entity.DocumentoEncargado,
                        @NombreEncargado = entity.NombreEncargado,
                        @SctrEncargado = entity.SctrEncargado,
                        @Consideracion = entity.Consideracion,
                        @UsuarioEdicion = entity.IdUsuarioEdicion
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<Formulario> ObtenerPorId(int id)
        {
            Formulario item = null;
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    using (SqlMapper.GridReader gridReader = await scope.QueryMultipleAsync("PU.Usp_GetFormularioProgramacion", new { IdFormulario = id }, commandType: CommandType.StoredProcedure))
                    {
                        item = await gridReader.ReadFirstOrDefaultAsync<Formulario>();
                        if (item == null)
                            throw new CustomException(string.Format("No existe el Id {0} en la tabla formulario", id));

                        item.ListaAdjunto = (await gridReader.ReadAsync<AdjuntoFormulario>()).ToList();
                        item.ListaConsideracion = (await gridReader.ReadAsync<FormularioConsideracion>()).ToList();
                        item.ListaArchivo = (await gridReader.ReadAsync<ArchivoAdjunto>()).ToList();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return item;
        }
        public async Task<StatusResponse> DeleteFormulario(int id)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_EliminarFormulario]",
                    new
                    {
                        @Formulario = id
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
        public async Task<StatusResponse> DeleteArchivoAdjunto(int formulario)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_EliminarArchivoAdjunto]",
                    new
                    {
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
        public async Task<StatusResponse> DeleteFormularioConsideracion(int formulario)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_EliminarFormularioConsideracion]",
                    new
                    {
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
        public async Task<StatusResponse> DeleteAdjuntoFormulario(int formulario)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_EliminarAdjuntoFormulario]",
                    new
                    {
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
        public async Task<StatusResponse> ValidaFormularioEdicion(int id)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[PU].[Usp_ValidaFormularioEdicion]",
                    new
                    {
                        @Formulario = id
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

    }
}
