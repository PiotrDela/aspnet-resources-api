using ResourcesManagementApi.Domain.Entities;

namespace ResourcesManagementApi.Authentication;

public interface IAuthenticationProvider
{
    Task<User> AuthenticateAsync(string username, string password);
}
