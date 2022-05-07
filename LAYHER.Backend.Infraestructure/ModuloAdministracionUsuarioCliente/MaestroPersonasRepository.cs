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
    public class MaestroPersonasRepository : IMaestroPersonasRepository
    {
        protected readonly ICustomConnection mConnection;

        public MaestroPersonasRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }

        public async Task<List<MaestroPersonas>> List(int id, int offset, int fetch)
        {
            List<MaestroPersonas> entity = new List<MaestroPersonas>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[COMUN].[Usp_ListaPersonas]",
                    new
                    {
                        @Persona = id,
                        @Offset = offset,
                        @Fetch = fetch
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<MaestroPersonas>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<List<MaestroPersonas>> ListCliente()
        {
            List<MaestroPersonas> entity = new List<MaestroPersonas>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[AUC].[Usp_ListaClientes]",
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<MaestroPersonas>)items;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<MaestroPersonas> DatosClienteByProgramacion(int programacion)
        {
            MaestroPersonas entity = new MaestroPersonas();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[AUC].[Usp_ProgramacionClienteDatos]",
                    new
                    {
                        @ProgramacionUnidad = programacion
                    }, commandType: CommandType.StoredProcedure);
                    entity = items.FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return entity;
        }

        public async Task<StatusReponse<MaestroPersonas>> SaveLAyherShop(MaestroPersonas entity)
        {
            StatusReponse<MaestroPersonas> status = new StatusReponse<MaestroPersonas>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[AUC].[Usp_SaveUsuarioLayherShop]",
                    new
                    {
                        @Documento = entity.Documento,
                        @TipoDocumento = entity.TipoDocumento,
                        @Nombre = entity.NombreCompleto + " " + entity.ApellidoPaterno,
                        @UsuarioCreacion = entity.IdUsuarioCreacion,
                        @Direccion = entity.Direccion,
                        @Clave = entity.Clave
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (MaestroPersonas)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }

        public async Task<StatusReponse<MaestroPersonas>> Save(MaestroPersonas entity)
        {
            StatusReponse<MaestroPersonas> status = new StatusReponse<MaestroPersonas>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<MaestroPersonas>("[AUC].[Usp_SaveUsuario]",
                    new
                    {
                        @Documento = entity.Documento,
                        @TipoDocumento = entity.TipoDocumento,
                        @Nombre = entity.NombreCompleto + " " + entity.ApellidoPaterno,
                        @UsuarioCreacion = entity.IdUsuarioCreacion,
                        @Direccion = entity.Direccion,
                        @Clave = entity.Clave
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (MaestroPersonas)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }

        public async Task<StatusReponse<EmpresaEmpleado>> SaveEmpresaEmpleado(EmpresaEmpleado entity)
        {
            StatusReponse<EmpresaEmpleado> status = new StatusReponse<EmpresaEmpleado>() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<EmpresaEmpleado>("[AUC].[Usp_SaveEmpresaEmpleado]",
                    new
                    {
                        @Empresa = entity.IdEmpresa,
                        @Empleado = entity.IdEmpleado,
                        @UsuarioCreacion = entity.IdUsuarioCreacion
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (EmpresaEmpleado)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return status;
        }

        public async Task<StatusResponse> Update(MaestroPersonas entity)
        {
            StatusResponse status = new StatusResponse() { Success = false, Title = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    await scope.QueryAsync("[AUC].[Usp_UpdateUsuario]",
                    new
                    {
                        @Persona = entity.Persona,
                        @Documento = entity.Documento,
                        @TipoDocumento = entity.TipoDocumento,
                        @Nombre = entity.Nombre,
                        @ApellidoP = entity.ApellidoPaterno,
                        @ApellidoM = entity.ApellidoMaterno,
                        @NombreCompleto = entity.NombreCompleto,
                        @UsuarioEdicion = entity.IdUsuarioEdicion,
                        @Clave = entity.Clave,
                        @Direccion = entity.Direccion,
                        @Email = entity.Correo
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

        public async Task<StatusResponse> DisableEnablePersona(string documento, string estado)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[AUC].[Usp_InhabilitarHabilitarUsuario]",
                    new
                    {
                        @Documento = documento,
                        @Estado = estado
                    }, commandType: CommandType.StoredProcedure);
                    var firstRow = items.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    response.Success = (bool)details[Heading[0]];
                }
                catch (Exception e)
                {

                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return response;
        }

        public async Task<StatusResponse> DeleteUsuario(string documento)
        {
            StatusResponse response = new StatusResponse { Success = false, Detail = "" };

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync("[AUC].[Usp_EliminarUsuario]",
                    new
                    {
                        @Documento = documento
                    }, commandType: CommandType.StoredProcedure);
                    var firstRow = items.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    response.Success = (bool)details[Heading[0]];
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación");
                }
            }
            return response;
        }
    }
}
