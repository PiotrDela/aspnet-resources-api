using Dapper;
using ResourcesManagementApi.Domain.Entities;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext dapperContext;

        public UserRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        public async Task<User> GetAsync(int id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using (var connection = dapperContext.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });
                return user;
            }
        }
    }
}
