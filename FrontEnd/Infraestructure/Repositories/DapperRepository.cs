using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Frontend.Repositories
{
    public abstract class DapperRepository
    {
        private readonly IConfiguration _configuration;
        protected DapperRepository(IConfiguration configuration) => this._configuration = configuration;
        private static SqlConnection GetConnection(string dbConnection) => new(dbConnection);

        public async Task<TOutput> GetFirstAsync<TOutput>(
            string NameProcedureOrQueryString, DynamicParameters parameters, CommandType typeCommand) where TOutput : new()
        {
            using var connection = GetConnection(this._configuration.GetConnectionString("connectionName"));
            var cmd = new CommandDefinition(NameProcedureOrQueryString, null, null, null, typeCommand);
            await connection.OpenAsync();
            if (parameters != null)
                cmd = new CommandDefinition(NameProcedureOrQueryString, parameters, null, null, typeCommand);
            var retorno = await connection.QueryFirstOrDefaultAsync<TOutput>(cmd);
            return retorno;
        }

        public async Task<IEnumerable<TOutput>> GetListAsync<TOutput>(string NameProcedureOrQueryString, DynamicParameters parameters, CommandType typeCommand) where TOutput : new()
        {
            using var connection = GetConnection(this._configuration.GetConnectionString("connectionName"));
            var cmd = new CommandDefinition(NameProcedureOrQueryString, parameters, null, null, typeCommand);
            await connection.OpenAsync();
            if (parameters != null)
                cmd = new CommandDefinition(NameProcedureOrQueryString, parameters, null, null, typeCommand);
            return await connection.QueryAsync<TOutput>(cmd);
        }

        public async Task<IEnumerable<TOutput>> GetAsyncTransaction<TOutput>(
            string NameProcedureOrQueryString, DynamicParameters parameters, CommandType typeCommand, IsolationLevel isolation) where TOutput : new()
        {
            using var connection = GetConnection(this._configuration.GetConnectionString("connectionName"));
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync(isolation);
            try
            {
                var result = await connection.QueryAsync<TOutput>(NameProcedureOrQueryString, parameters, transaction, null, typeCommand);
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                throw new Exception(e.Message);
            }
        }
    }
}

