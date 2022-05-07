using Dapper;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloAdministracionUsuarioCliente
{
    public class PermisoRepository : IPermisoRepository
    {
        protected readonly ICustomConnection mConnection;

        public PermisoRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<Permiso>> List(int id)
        {
            List<Permiso> entity = new List<Permiso>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Permiso>("[AUC].[Usp_ListaPermisos]",
                    new
                    {
                        @Permiso = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<Permiso>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<PermisoPerfilPersona>> ListPermisoPerfilPersona(int id)
        {
            List<PermisoPerfilPersona> entity = new List<PermisoPerfilPersona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PermisoPerfilPersona>("[AUC].[Usp_ListaPermisoPerfilPersona]",
                    new
                    {
                        @PermisoPerfilPersona = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<PermisoPerfilPersona>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<PermisoPerfilPersona>> ListPermisoProyectoPerfilPersona(int persona,int perfil, string afe, string documento, string nombre)
        {
            List<PermisoPerfilPersona> entity = new List<PermisoPerfilPersona>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PermisoPerfilPersona>("[AUC].[Usp_ListaPermisoProyectoPerfilPersona]",
                    new
                    {
                        @Persona= persona,
                        @Perfil = perfil,
                        @Proyecto = afe,
                        @Documento = documento,
                        @Nombre = nombre
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<PermisoPerfilPersona>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<StatusReponse<PermisoPerfilPersona>> SavePermisoPerfilPersona(PermisoPerfilPersona entity)
        {
            StatusReponse<PermisoPerfilPersona> status = new StatusReponse<PermisoPerfilPersona>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<PermisoPerfilPersona>("[AUC].[Usp_SavePermisoPerfilUsuario]",
                    new
                    {
                        @PerfilPersona = entity.IdPerfilPersona,
                        @Permiso = entity.IdPermiso,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (PermisoPerfilPersona)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<StatusResponse> DeletePermisoPerfilPersona(int id)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[AUC].[Usp_DeletePermisoPerfilUsuario]",
                    new
                    {
                        @PerfilPersona = id
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
        //public async Task<StatusReponse> UpdatePermisoPerfilPersona(PermisoPerfilPersona entity)
        //{
        //    StatusReponse status = new StatusReponse() { Success = false, Title = "" };
        //    string consulta = "Update AUC.Permiso_Perfil_Persona " +
        //                      "set IdPerfil_Persona = @IdPerfilPersona, " +
        //                      "IdPermiso = @IdPermiso, " +
        //                      "Estado = @Estado " +
        //                      "Where IdPermiso_Perfil_Persona = @IdPermisoPerfilPersona"; ;
        //    consulta = consulta.Replace("@IdPermisoPerfilPersona", entity.IdPermisoPerfilPersona.ToString());
        //    consulta = consulta.Replace("@IdPerfilPersona", entity.IdPerfilPersona.ToString());
        //    consulta = consulta.Replace("@IdPermiso", entity.IdPermiso.ToString());

        //    using (var scope = await mConnection.BeginConnection())
        //    {
        //        try
        //        {
        //            await scope.QueryAsync(consulta);
        //            status.Success = true;
        //        }
        //        catch (Exception e)
        //        {

        //            throw new CustomException("Sucedió un error al realizar la operación");
        //        }
        //    }
        //    return status;
        //}
    }
}
