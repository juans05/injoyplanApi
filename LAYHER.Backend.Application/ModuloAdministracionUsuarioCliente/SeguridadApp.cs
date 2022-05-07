using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
//using LAYHER.Backend.Domain.Seguridad;
using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LAYHER.Backend.Application.Shared;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain;
using LAYHER.Backend.Domain.Notificacion.Interfaces;

namespace LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente
{
    public class SeguridadApp : BaseApp<SeguridadApp>
    {
        private ISeguridadRepository _seguridadRepository;
        private IUsuarioRepository _usuarioRepository;
        private IMailManager _mailManager;
        public SeguridadApp(
            ISeguridadRepository seguridadRepository,
            IUsuarioRepository usuarioRepository,
            IMailManager mailManager,
            ILogger<BaseApp<SeguridadApp>> logger) : base()
        {
            this._seguridadRepository = seguridadRepository;
            this._usuarioRepository = usuarioRepository;
            this._mailManager = mailManager;
        }

        public async Task<StatusReponse<UsuarioPersona>> ValidaUsuario(LoginRequest request)
        {
            return await _seguridadRepository.ValidaUsuario(request);
        }

        public async Task<StatusResponse> CambioContrasena(MaestroPersonas entity)
        {
            StatusResponse response = new StatusResponse();

            using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                response = await _seguridadRepository.CambioContrasena(entity);
                if (response.Success)
                {
                    response.Detail = "Operacion exitosa...";
                    tran.Complete();
                }
            }
            return response;
        }

        public async Task<StatusResponse> validarTokenEnListaNegra(string token)
        {
            return await _seguridadRepository.ValidaListaNegra(token);
        }

        public async Task<StatusReponse<ListaNegra>> SaveListaNegra(ListaNegra entity)
        {
            StatusReponse<ListaNegra> response = new StatusReponse<ListaNegra>();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _seguridadRepository.SaveListaNegra(entity);
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

        public async Task<StatusReponse<UsuarioCliente>> ValidaUsuarioCliente(UsuarioCliente request)
        {
            return await _seguridadRepository.ValidaUsuarioCliente(request);
        }

        public async Task<StatusReponse<UsuarioPersona>> ObtenerUsuarioInternoPorId(int idUsuario)
        {
            return await _seguridadRepository.ObtenerUsuarioInternoPorId(idUsuario);
        }

        public async Task<StatusResponse> RecuperarContrasena(string nroDocumento)
        {
            StatusResponse status = null;
            Empresa empresa = null;
            try
            {
                empresa = await _usuarioRepository.ObtenerEmpresaPorNroDocumento(nroDocumento);
                if (empresa == null)
                    throw new Exception("No existe datos de la empresa");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "");
                status = new StatusResponse(false, "No se pudo completar la operación");
                return status;
            }

            if (string.IsNullOrEmpty(empresa.CorreoElectronico))
            {
                status = new StatusResponse(false, "Su cuenta no tiene configurado un correo electrónico. No se pudo completar la operación");
                return status;
            }

            //cambiar contraseña
            string nuevaClave = StringUtils.RandomPassword();
            bool reseteoClave = await _seguridadRepository.ResetearContrasena(empresa.Id, nuevaClave);

            //enviar por email la nueva contraseña
            bool seEnvioCorreo = false;
            try
            {

                await this._mailManager.SendEmail("LayherSuite (App móvil) - Recuperar contraseña", "Estimado usuario, usted ha solicitado una nueva contraseña desde la aplicación móvil de Layher.\nSu nueva contraseña es: " + nuevaClave, new List<string>() { empresa.CorreoElectronico }, isHTML: false);
                seEnvioCorreo = true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "correo: {0}", empresa.CorreoElectronico);
            }

            //validar que salió el correo
            if (!seEnvioCorreo)
            {
                return new StatusResponse(false, "El correo electrónico asociado a su cuenta no es válido. Contacte con el equipo Layher por otro medio.");
            }

            string correoObfuscado = StringUtils.ObfuscateEmail(empresa.CorreoElectronico);
            status = new StatusResponse(true, "Su nueva contraseña a sido enviado a su correo " + correoObfuscado);
            return status;
        }
    }
}
