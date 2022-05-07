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
    public class MaestroProyectosRepository : IMaestroProyectosRepository
    {
        protected readonly ICustomConnection mConnection;

        public MaestroProyectosRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<MaestroProyectos>> List(string id, int offset, int fetch)
        {
            List<MaestroProyectos> entity = new List<MaestroProyectos>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroProyectos>("[AUC].[Usp_ListaProyectos]",
                    new
                    {
                        @Proyecto = id,
                        @OffSet = offset,
                        @Fetch = fetch
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<MaestroProyectos>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<MaestroProyectos>> ListProyectoCliente(string id)
        {
            List<MaestroProyectos> entity = new List<MaestroProyectos>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroProyectos>("[AUC].[Usp_ListaProyectoCliente]",
                    new
                    {
                        @Proyecto = id
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<MaestroProyectos>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<List<MaestroProyectos>> ListByPersona(int persona, string proyectos, string afe, string localname, int zona, int offset, int fetch)
        {
            List<MaestroProyectos> entity = new List<MaestroProyectos>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroProyectos>("[AUC].[Usp_ListaProyectosxPersona]",
                    new
                    {
                        @Persona = persona,
                        @Proyecto = afe,
                        @ListaProyectos = proyectos,
                        @LocalName = localname,
                        @Zona = zona,
                        @OffSet = offset,
                        @Fetch = fetch
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<MaestroProyectos>)items;
                }
                catch (Exception e) 
                { 
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }
        public async Task<StatusReponse<ProyectoPerfilPersona>> SaveProyectoPerfilPersona(ProyectoPerfilPersona entity)
        {
            StatusReponse<ProyectoPerfilPersona> status = new StatusReponse<ProyectoPerfilPersona>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ProyectoPerfilPersona>("[AUC].[Usp_SaveProyectoPerfilUsuario]",
                    new
                    {
                        @PerfilPersona = entity.IdPerfilPersona,
                        @Proyecto = entity.afe,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (ProyectoPerfilPersona)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }
        public async Task<StatusResponse> DeleteProyectoPerfilPersona(int id)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[AUC].[Usp_DeleteProyectoPerfilUsuario]",
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
        //public async Task<StatusReponse> UpdateProyectoPerfilPersona(ProyectoPerfilPersona entity)
        //{
        //    StatusReponse status = new StatusReponse() { Success = false, Title = "" };
        //    string consulta = "Update AUC.Proyecto_Perfil_Persona " +
        //                      "set IdPerfil_Persona = @IdPerfil_Persona, " +
        //                      "Afe = '@afe' " +
        //                      "Where IdProyecto_Perfil_Persona = @IdProyecto_PerfilPersona"; ;
        //    consulta = consulta.Replace("@IdProyectoPerfilPersona", entity.IdProyectoPerfilPersona.ToString());
        //    consulta = consulta.Replace("@IdPerfilPersona", entity.IdPerfilPersona.ToString());
        //    consulta = consulta.Replace("@Afe", entity.afe);

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
        //public async Task<StatusReponse<MaestroProyectos>> UpdateProyectoStatus(string estado, string afe)
        //{
        //    StatusReponse<MaestroProyectos> status = new StatusReponse<MaestroProyectos>() { Success = false, Title = "" };
        //    string consulta = "Update CB_MestroProyectos set Status = '@estado' Where Afe = '@afe'";
        //    consulta = consulta.Replace("@estado", estado);
        //    consulta = consulta.Replace("@afe", afe);

        //    using (var scope = await mConnection.BeginConnection())
        //    {
        //        try
        //        {
        //            var items = await scope.QueryAsync<MaestroProyectos>(consulta);
        //            status.Data = (MaestroProyectos)items.FirstOrDefault();
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
