using Dapper;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Domain;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure.ModuloAdministracionUsuarioCliente
{
    public class UsuarioRepository : IUsuarioRepository
    {
        protected readonly ICustomConnection mConnection;
        protected readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(ICustomConnection _connection, ILogger<UsuarioRepository> logger)
        {
            this.mConnection = _connection;
            this._logger = logger;
        }

        public async Task<Empresa> ObtenerEmpresaPorNroDocumento(string nroDocumento)
        {
            Empresa item = null;

            try
            {
                using (var scope = await mConnection.BeginConnection())
                {
                    item = await scope.QueryFirstOrDefaultAsync<Empresa>("AUC.USP_OBTENER_CLIENTE_POR_DOCUMENTO",
                    new
                    {
                        NroDocumento = nroDocumento,
                    }, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw new CustomException("No se pudo obtener los datos de la empresa", ex);
            }

            return item;
        }

        public async Task<Usuario> ObtenerUsuarioPorNroDocumento(string nroDocumento)
        {
            Usuario item = null;

            try
            {
                using (var scope = await mConnection.BeginConnection())
                {
                    item = await scope.QueryFirstOrDefaultAsync<Usuario>("AUC.Usp_ObtenerUsuarioPorNroDocumento",
                    new
                    {
                        NroDocumento = nroDocumento,
                    }, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw new CustomException("No se pudo obtener los datos del usuario", ex);
            }

            return item;
        }
    }
}
