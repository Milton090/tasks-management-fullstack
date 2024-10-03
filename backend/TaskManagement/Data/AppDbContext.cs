using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskManagement.Data
{
    public class AppDbContext
    {
        private readonly IConfiguration _config;
        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("SQLConnection"));
        }

        public IEnumerable<T> LoadData<T>(String sql, Object? parameters)
        {
            using (IDbConnection dbConn = CreateConnection())
            {
                return dbConn.Query<T>(sql, parameters);
            }
        }

        public Boolean Execute(String sql, Object? parameters)
        {
            using (IDbConnection dbConn = CreateConnection())
            {
                dbConn.Open();
                return dbConn.Execute(sql, parameters) > 0;
            }
        }
    }
}
