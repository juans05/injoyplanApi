using Dapper;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
//using LAYHER.Backend.Domain.Seguridad;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloAdministracionUsuarioCliente
{
    public class SeguridadRepository : ISeguridadRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<SeguridadRepository> _logger;

        public SeguridadRepository(ICustomConnection _connection, ILogger<SeguridadRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }

        public async Task<StatusReponse<UsuarioPersona>> ValidaUsuario(LoginRequest request)
        {
            StatusReponse<UsuarioPersona> status = new StatusReponse<UsuarioPersona>() { Success = false, Title = "" };

            try
            {
                using (var scope = await mConnection.BeginConnection())
                {
                    var items = await scope.QueryAsync<UsuarioPersona>("Usp_ValidaPersona",
                    new
                    {
                        @usuario = request.document,
                        @clave = request.password                        
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (UsuarioPersona)items.FirstOrDefault();
                    if (status.Data == null)
                    {
                        status.Success = false;
                        status.Title = "Usuario o Contrasena incorrecta...";
                    }
                    else {
                        status.Success = true;
                    }                    
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "parametros de cadena de conexión inválidos");
                status.Title = "Los datos ingresados son incorrectos";
                return status;
            }
            return status;
        }

        public async Task<StatusReponse<UsuarioPersona>> ObtenerUsuarioInternoPorId(int idUsuario)
        {
            StatusReponse<UsuarioPersona> status = new StatusReponse<UsuarioPersona>() { Success = false, Title = "" };

            try
            {
                using (var scope = await mConnection.BeginConnection())
                {
                    status.Data = await scope.QueryFirstOrDefaultAsync<UsuarioPersona>("[AUC].[Usp_ObtenerUsuarioInternoPorId]",
                    new
                    {
                        @Persona = idUsuario
                    }, commandType: CommandType.StoredProcedure);
                    
                    if (status.Data == null)
                    {
                        status.Success = false;
                        status.Title = "Usuario no existe";
                    }
                    else
                    {
                        status.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "parametros de cadena de conexión inválidos");
                status.Title = "Los datos ingresados son incorrectos";
                return status;
            }
            return status;
        }

        

        public async Task<StatusResponse> ValidaListaNegra(string token)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.ExecuteScalarAsync("[AUC].[Usp_ValidaListaNegra]",
                    new
                    {
                        @Token = token
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = (bool)items;
                }
                catch (Exception ex)
                {
                    throw new CustomException("Error", ex);
                }
            }
            return status;
        }

        public async Task<StatusReponse<ListaNegra>> SaveListaNegra(ListaNegra entity)
        {
            StatusReponse<ListaNegra> status = new StatusReponse<ListaNegra>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ListaNegra>("[AUC].[Usp_SaveListaNegra]",
                    new
                    {
                        @Token = entity.Token,
                        @UsuarioCreacion = entity.IdUsuarioCreacion,
                        @FechaCreacion = entity.FechaCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (ListaNegra)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }

        public async Task<StatusResponse> CambioContrasena(MaestroPersonas entity)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[AUC].[Usp_CambioContrasena]",
                    new
                    {
                        @Documento = entity.Documento,
                        @NuevaClave = entity.NuevaClave,
                        @AntiguaClave = entity.Clave
                    }, commandType: CommandType.StoredProcedure);
                    var firstRow = items.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    status.Success = (bool)details[Heading[0]];
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }

        public Task<bool> ResetearContrasena(int idUsuario, string clave)
        {
            throw new NotImplementedException();
        }

        public Task<StatusReponse<UsuarioCliente>> ValidaUsuarioCliente(UsuarioCliente resquest)
        {
            throw new NotImplementedException();
        }
    }
}
