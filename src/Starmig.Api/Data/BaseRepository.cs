using Dapper;
using Microsoft.Data.SqlClient;

namespace Starmig.Api.Data
{
    public abstract class BaseRepository
    {
        private readonly DapperContext _context;
        protected BaseRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> ExecuteSql(string sql, object parameters)
        {
            using var connection = _context.CreateConnection();
            return (await connection.ExecuteAsync(sql: sql, param: parameters)) > 0;
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, object parameters)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<T>(sql: sql, param: parameters);
        }

        public async Task<T> QueryFirstOrDefault<T>(string sql, object parameters)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql: sql, param: parameters);
        }
    }
}
