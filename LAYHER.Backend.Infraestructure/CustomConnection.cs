using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Infraestructure
{
    public class CustomConnection : ICustomConnection
    {
        private string connectionString;
        private MySqlConnection con;
        private MySqlTransaction trx;

        public CustomConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbTransaction GetTransaction() => trx;

        public async Task<IDbConnection> BeginConnection(bool useTransaction = false)
        {
            if (this.con == null)
                this.con = new MySqlConnection(this.connectionString);

            if (this.con.State != System.Data.ConnectionState.Open)
            {
                if (string.IsNullOrEmpty(this.con.ConnectionString))
                    this.con.ConnectionString = this.connectionString;

                await this.con.OpenAsync();
            }

            if (useTransaction)
            {
                //this.trx = this.con.BeginTransaction();
                this.trx = await Task.Run<MySqlTransaction>(
                    () => con.BeginTransaction()
                );
            }

            return this.con;
        }

        public async Task Complete()
        {
            if (trx != null)
            {
                //this.trx.Commit();
                await Task.Run(() =>
                {
                    if (trx.Connection != null)
                        trx.Commit();
                });
            }

            await this.CloseConnection();

            SqlConnection.ClearAllPools();
        }

        public async Task Rollback()
        {
            if (trx != null)
            {
                //this.trx.Rollback();
                await Task.Run(() => { trx.Rollback(); });
            }

            //await this.CloseConnection();
        }

        public async Task CloseConnection()
        {
            await Task.Run(() =>
            {
                if (trx != null)
                    this.trx.Dispose();

                this.con.Close();
                this.con.Dispose();
            });
        }

        #region Command Async...
        public async Task ExecuteCommandAsync(string commandText, params DbParameter[] values)
        {
            if (con == null)
                await BeginConnection();

            var cmd = new MySqlCommand(commandText, this.con);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;

            if (this.trx != null)
                cmd.Transaction = this.trx;

            cmd.Parameters.AddRange(values);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ExecuteCommandAsync(string commandText, string name, object value)
        {
            await ExecuteCommandAsync(commandText, new SqlParameter(name, value));
        }
        #endregion

        public async Task<IDataReader> ExecuteReaderAsync(string commandText, params DbParameter[] values)
        {
            if (con == null)
                await BeginConnection();

            var cmd = new MySqlCommand(commandText, this.con);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;

            if (this.trx != null)
                cmd.Transaction = this.trx;

            cmd.Parameters.AddRange(values);

            var dr = await cmd.ExecuteReaderAsync();

            return dr;
        }

        public async Task<IDataReader> ExecuteReaderAsync(string commandText, string name, object value)
        {
            return await ExecuteReaderAsync(commandText, new SqlParameter(name, value));
        }
    }
}
