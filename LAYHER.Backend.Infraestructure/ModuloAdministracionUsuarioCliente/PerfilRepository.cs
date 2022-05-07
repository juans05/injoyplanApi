using System;
using System.Collections.Generic;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using Dapper;
using LAYHER.Backend.Shared;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Logging;

namespace LAYHER.Backend.Infraestructure.ModuloAdministracionUsuarioCliente
{
    public class PerfilRepository : IPerfilRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<PerfilRepository> _logger;

        public PerfilRepository(ICustomConnection _connection, ILogger<PerfilRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }

        public async Task<List<Perfil>> List(int id)
        {
            List<Perfil> entity = new List<Perfil>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Perfil>("[AUC].[Usp_ListaPerfilesUsuario]",
                    new
                    {
                        @Perfil = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Perfil>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<Perfil>> ListByDocumento(string documento)
        {
            List<Perfil> entity = new List<Perfil>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Perfil>("[AUC].[Usp_ListaPerfilesxDocumento]",
                    new
                    {
                        @Documento = documento
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Perfil>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<PerfilPersona>> ListPerfilPersona(int id)
        {
            List<PerfilPersona> entity = new List<PerfilPersona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PerfilPersona>("[AUC].[Usp_ListaPerfilPersona]",
                    new
                    {
                        @PerfilPersona = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<PerfilPersona>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<PerfilPermiso>> ListPerfilPermiso(int perfil, int permiso)
        {
            List<PerfilPermiso> entity = new List<PerfilPermiso>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PerfilPermiso>("[AUC].[Usp_ListaPerfilPermiso]",
                    new
                    {
                        @Perfil = perfil,
                        @Permiso = permiso
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<PerfilPermiso>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<StatusReponse<PerfilPersona>> SavePerfilPersona(PerfilPersona entity)
        {
            StatusReponse<PerfilPersona> status = new StatusReponse<PerfilPersona>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PerfilPersona>("[AUC].[Usp_SavePerfilUsuario]",
                    new
                    {
                        @Perfil = entity.IdPerfil,
                        @Persona = entity.IdPersona,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (PerfilPersona)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", e);
                }
            }
            return status;
        }

        public async Task<StatusResponse> UpdatePerfilPersona(PerfilPersona entity)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[AUC].[Usp_UpdatePerfilUsuario]",
                    new
                    {
                        @PerfilPersona = entity.IdPerfilPersona,
                        @Perfil = entity.IdPerfil,
                        @Persona = entity.IdPersona,
                        @UsuarioEdicion = entity.IdUsuarioEdicion
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                }
                catch (Exception e)
                {

                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }

    }
}
