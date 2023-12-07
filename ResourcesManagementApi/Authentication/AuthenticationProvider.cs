using Dapper;
using ResourcesManagementApi.Domain.Entities;
using ResourcesManagementApi.Infrastructure;

namespace ResourcesManagementApi.Authentication;

public class AuthenticationProvider: IAuthenticationProvider
{
    private readonly DapperContext dapperContext;

    public AuthenticationProvider(DapperContext dapperContext)
    {
        this.dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
    }

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var query = "SELECT * FROM Users WHERE Name = @Name AND Password = @Password"; // obviosuly we should compare hashes here instaed of plain text

        using (var connection = dapperContext.CreateConnection())
        {
            var resource = await connection.QuerySingleOrDefaultAsync<User>(query, new { Name = username, Password = password });
            return resource;
        }
    }
}
