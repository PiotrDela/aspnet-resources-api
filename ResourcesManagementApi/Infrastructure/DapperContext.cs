using Microsoft.Data.SqlClient;
using System.Data;

namespace ResourcesManagementApi.Infrastructure
{
    public class DapperContext
    {
        private readonly string connectionString;

        public DapperContext(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            this.connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(this.connectionString);
        }
    }
}
