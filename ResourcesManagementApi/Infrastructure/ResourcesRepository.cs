using Dapper;
using ResourcesManagementApi.Domain.Entities;
using ResourcesManagementApi.Domain.Repositories;
using System.Data;

namespace ResourcesManagementApi.Infrastructure
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly DapperContext dapperContext;

        public ResourceRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        public async Task<int> AddAsync(Resource resource)
        {
            var command = "INSERT INTO Resources (Status) OUTPUT INSERTED.Id VALUES (@Status)";

            var parameters = new DynamicParameters();
            parameters.Add("Status", ResourceAvaiabilityStatus.Available, DbType.Int32);
            
            using (var connection = dapperContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(command, parameters);
                return id;
            }
        }

        public async Task<Resource> GetAsync(int id)
        {
            var query = "SELECT * FROM Resources WHERE Id = @Id";

            using (var connection = dapperContext.CreateConnection())
            {
                var resource = await connection.QuerySingleOrDefaultAsync<Resource>(query, new { id });
                return resource;
            }
        }

        public async Task UpdateAsync(Resource resource)
        {
            byte[] rowVersion = null;

            var command = "UPDATE Resources SET Status = @Status, LockExpirationTimeUtc = @LockExpirationTime, LockedById = @LockedById OUTPUT inserted.Version WHERE Id = @Id AND Version = @Version"; // TODO could replace names of properties with nameof() operator not to repeat strings

            var parameters = new DynamicParameters();
            parameters.Add("Id", resource.Id, DbType.Int32);
            parameters.Add("Status", resource.Status, DbType.Int32);
            parameters.Add("LockExpirationTime", resource.LockExpirationTimeUtc, DbType.DateTime);
            parameters.Add("LockedById", resource.LockedById, DbType.Int32);
            parameters.Add("Version", resource.Version);

            using (var connection = dapperContext.CreateConnection())
            {
                rowVersion = await connection.ExecuteScalarAsync<byte[]>(command, parameters);
            }

            if (rowVersion == null)
            {
                throw new DBConcurrencyException("Entity you were trying to edit has changed. Try again.");
            }
        }
    }
}
