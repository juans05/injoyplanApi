using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente
{
    public class AdministracionUsuarioClienteApp : BaseApp<AdministracionUsuarioClienteApp>
    {
        private IMaestroPersonasRepository _maestroPersonasRepository;
        private IMaestroProyectosRepository _maestroProyectosRepository;
        private IPerfilRepository _perfilRepository;
        private IPermisoRepository _permisoRepository;

        public AdministracionUsuarioClienteApp(IMaestroPersonasRepository maestroPersonasRepository, 
                                               IMaestroProyectosRepository maestroProyectosRepository, 
                                               IPerfilRepository perfilRepository, 
                                               IPermisoRepository permisoRepository,
                                               ILogger<BaseApp<AdministracionUsuarioClienteApp>> logger) : base()
        {
            this._maestroPersonasRepository = maestroPersonasRepository;
            this._maestroProyectosRepository = maestroProyectosRepository;
            this._perfilRepository = perfilRepository;
            this._permisoRepository = permisoRepository;
        }
        public async Task<StatusReponse<List<MaestroProyectos>>> ListByPersona(int persona, string proyectos, string afe, string localname, int zona, int offset, int fetch)
        {
            StatusReponse<List<MaestroProyectos>> status = null;
            if (string.IsNullOrEmpty(afe))
            {
                status = new StatusReponse<List<MaestroProyectos>>(false, "Debe especificar el Proyecto.");
                return status;
            }
            status = await this.ComplexResponse(() => _maestroProyectosRepository.ListByPersona(persona, proyectos, afe, localname, zona, offset, fetch));
            return status;
        }
        public async Task<StatusResponse> Save(PermisoPerfilPersona entity)
        {
            StatusReponse<MaestroPersonas> maestroPersona = new StatusReponse<MaestroPersonas>() { Success = false, Title = "" };
            StatusReponse<PerfilPersona> perfilPersona = new StatusReponse<PerfilPersona>() { Success = false, Title = "" };
            StatusReponse<PermisoPerfilPersona> permisoPerfilPersona = new StatusReponse<PermisoPerfilPersona>() { Success = false, Title = "" };
            StatusReponse<ProyectoPerfilPersona> proyectoPerfilPersona = new StatusReponse<ProyectoPerfilPersona>() { Success = false, Title = "" };
            StatusReponse<EmpresaEmpleado> empresaEmpleado = new StatusReponse<EmpresaEmpleado>() { Success = false, Title = "" };
            StatusResponse respuesta = new StatusResponse() { Success = false, Title= "", Detail = "Error..."};

            try
            {
                using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    MaestroPersonas mPersona = new MaestroPersonas();
                    mPersona.IdAccountLogin = entity.IdAccountLogin;
                    mPersona.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;
                    mPersona.TipoDocumento = entity.TipoDocumento;
                    mPersona.Documento = entity.Documento;
                    mPersona.Clave = entity.Clave;
                    mPersona.NombreCompleto = entity.NombreCompleto;
                    //Se realiza el registro del cliente en la table Persona
                    maestroPersona = await _maestroPersonasRepository.Save(mPersona);
                    if (maestroPersona.Success)
                    {
                        respuesta.Detail = "Error en el registro del Perfil...";
                        tran1.Complete();
                    }
                }
                if (maestroPersona.Success)
                {
                    using (var tran2 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        EmpresaEmpleado eEmpleado = new EmpresaEmpleado();
                        eEmpleado.IdEmpresa = entity.IdAccountLogin;
                        eEmpleado.IdEmpleado = maestroPersona.Data.Persona;
                        eEmpleado.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;
                        //Registra los hijos del cliente en la tabla seg.empresaempleado
                        empresaEmpleado = await _maestroPersonasRepository.SaveEmpresaEmpleado(eEmpleado);
                        if (empresaEmpleado.Success)
                        {
                            respuesta.Detail = "Error en el Registro del Usuario...";
                            tran2.Complete();
                        }
                    }

                }
                if (empresaEmpleado.Success)
                {
                    using (var tran3 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        PerfilPersona pPersona = new PerfilPersona();
                        pPersona.IdPerfil = entity.IdPerfil;
                        pPersona.IdPersona = maestroPersona.Data.Persona;
                        pPersona.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;//Aca se debera poner el ID del usaurio que realiza el registro del cliente.
                                                                                     //Registro de la relacion entre el Perfil y Permiso
                        perfilPersona = await _perfilRepository.SavePerfilPersona(pPersona);
                        if (perfilPersona.Success)
                        {
                            respuesta.Detail = "Error en el Registro de Permisos...";
                            tran3.Complete();
                        }
                    }

                }
                if (perfilPersona.Success)
                {
                    using (var tran4 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var item in entity.Permisos)
                        {
                            PermisoPerfilPersona pPerfilPersona = new PermisoPerfilPersona();
                            pPerfilPersona.IdPerfilPersona = perfilPersona.Data.IdPerfilPersona;
                            pPerfilPersona.IdPermiso = item.IdPermiso;
                            pPerfilPersona.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;//Aca se debera poner el ID del usaurio que realiza el registro del cliente.
                                                                                               //Registro de la relacion entre el Perfil y Persona
                            permisoPerfilPersona = await _permisoRepository.SavePermisoPerfilPersona(pPerfilPersona);
                            if (!permisoPerfilPersona.Success)
                            {
                                break;
                            }
                        }
                        //Si es Reponsable(1) u Otro(6) se registra proyectos
                        if (entity.IdPerfil == 1 || entity.IdPerfil == 6)
                        {
                            foreach (var item in entity.Proyectos)
                            {
                                ProyectoPerfilPersona pyPerfilPersona = new ProyectoPerfilPersona();
                                pyPerfilPersona.IdPerfilPersona = perfilPersona.Data.IdPerfilPersona;
                                pyPerfilPersona.afe = item.Afe;
                                pyPerfilPersona.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;//Aca se debera poner el ID del usaurio que realiza el registro del cliente.

                                //Se registra la relacion entre el Proyecto-PerfilPersona
                                proyectoPerfilPersona = await _maestroProyectosRepository.SaveProyectoPerfilPersona(pyPerfilPersona);
                                if (!proyectoPerfilPersona.Success)
                                {
                                    break;
                                }
                            }
                            if (proyectoPerfilPersona.Success && permisoPerfilPersona.Success)
                            {
                                respuesta.Success = true;
                                respuesta.Detail = "Operacion exitosa...";
                                tran4.Complete();
                            }
                        }
                        else
                        {
                            if (permisoPerfilPersona.Success)
                            {
                                respuesta.Success = true;
                                respuesta.Detail = "Operacion exitosa...";
                                tran4.Complete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuesta;
        }
        public async Task<StatusResponse> Update(PermisoPerfilPersona entity)
        {
            StatusResponse maestroPersona = new StatusResponse() { Success = false, Title = "" };
            StatusResponse perfilPersona = new StatusResponse() { Success = false, Title = "" };
            StatusReponse<PermisoPerfilPersona> permisoPerfilPersona = new StatusReponse<PermisoPerfilPersona>() { Success = false, Title = "" };
            StatusReponse<ProyectoPerfilPersona> proyectoPerfilPersona = new StatusReponse<ProyectoPerfilPersona>() { Success = false, Title = "" };
            StatusResponse respuesta = new StatusResponse() { Success = false, Title = "", Detail = "Error..." };

            try
            {
                using (var tran1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    MaestroPersonas mPersona = new MaestroPersonas();
                    mPersona.IdAccountLogin = entity.IdAccountLogin;
                    mPersona.IdUsuarioEdicion = entity.IdUsuarioPersonaCreacion;
                    mPersona.Persona = entity.IdPersona;
                    mPersona.TipoDocumento = entity.TipoDocumento;
                    mPersona.Documento = entity.Documento;
                    mPersona.Clave = entity.Clave;
                    mPersona.NombreCompleto = entity.NombreCompleto;

                    PerfilPersona pPersona = new PerfilPersona();
                    pPersona.IdPerfilPersona = entity.IdPerfilPersona;
                    pPersona.IdPerfil = entity.IdPerfil;
                    pPersona.IdPersona = entity.IdPersona;
                    pPersona.IdUsuarioEdicion = entity.IdUsuarioPersonaCreacion;

                    //Se realiza el registro del cliente en la table Persona
                    maestroPersona = await _maestroPersonasRepository.Update(mPersona);

                    //Registro de la relacion entre el Perfil y Permiso
                    perfilPersona = await _perfilRepository.UpdatePerfilPersona(pPersona);
                    if (perfilPersona.Success && maestroPersona.Success)
                    {
                        respuesta.Success = true;
                        tran1.Complete();
                    }
                }
                if (perfilPersona.Success && maestroPersona.Success)
                {
                    respuesta.Success = false;
                    using (var tran2 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        StatusResponse deletePermiso = await _permisoRepository.DeletePermisoPerfilPersona(entity.IdPerfilPersona);
                        StatusResponse deleteProyecto = await _maestroProyectosRepository.DeleteProyectoPerfilPersona(entity.IdPerfilPersona);
                        if (deletePermiso.Success && deleteProyecto.Success)
                        {
                            foreach (var item in entity.Permisos)
                            {
                                PermisoPerfilPersona pPerfilPersona = new PermisoPerfilPersona();
                                pPerfilPersona.IdPerfilPersona = entity.IdPerfilPersona;
                                pPerfilPersona.IdPermiso = item.IdPermiso;
                                pPerfilPersona.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;
                                //Registro de la relacion entre el Perfil y Persona
                                permisoPerfilPersona = await _permisoRepository.SavePermisoPerfilPersona(pPerfilPersona);
                                if (!permisoPerfilPersona.Success)
                                {
                                    break;
                                }
                            }
                            //Si es Reponsable(1) u Otro(6) se registra proyectos
                            if (entity.IdPerfil == 1 || entity.IdPerfil == 6)
                            {
                                foreach (var item in entity.Proyectos)
                                {
                                    ProyectoPerfilPersona pyPerfilPersona = new ProyectoPerfilPersona();
                                    pyPerfilPersona.IdPerfilPersona = entity.IdPerfilPersona;
                                    pyPerfilPersona.afe = item.Afe;
                                    pyPerfilPersona.IdUsuarioCreacion = entity.IdUsuarioPersonaCreacion;//Aca se debera poner el ID del usaurio que realiza el registro del cliente.

                                    //Se registra la relacion entre el Proyecto-PerfilPersona
                                    proyectoPerfilPersona = await _maestroProyectosRepository.SaveProyectoPerfilPersona(pyPerfilPersona);
                                    if (!proyectoPerfilPersona.Success)
                                    {
                                        break;
                                    }
                                }
                                if (proyectoPerfilPersona.Success && permisoPerfilPersona.Success)
                                {
                                    respuesta.Success = true;
                                    respuesta.Detail = "Operacion exitosa...";
                                    tran2.Complete();
                                }
                            }
                            else
                            {
                                if (permisoPerfilPersona.Success)
                                {
                                    respuesta.Success = true;
                                    respuesta.Detail = "Operacion exitosa...";
                                    tran2.Complete();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return respuesta;
        }
        public async Task<StatusResponse> DisableEnablePersona(string documento, string estado)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _maestroPersonasRepository.DisableEnablePersona(documento, estado);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        public async Task<StatusResponse> DeleteUsuario(string documento)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _maestroPersonasRepository.DeleteUsuario(documento);
                    if (response.Success)
                    {
                        response.Detail = "Operacion exitosa...";
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
