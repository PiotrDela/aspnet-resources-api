using ResourcesManagementApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ResourcesManagementApi.Authentication;

public class UserAuthenticationRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}

//public class AuthenticationProvider
//{
//    public User
//}
