using System.ComponentModel.DataAnnotations;

namespace ResourcesManagementApi.Authentication;

public class UserAuthenticationRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
