using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourcesManagementApi.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ResourcesManagementApi.Controllers;

[ApiController]
[Route("[controller]")]
public partial class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationProvider authenticationProvider;

    public AuthenticationController(IAuthenticationProvider authenticationProvider)
    {
        this.authenticationProvider = authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("token")]
    public async Task<IActionResult> Authenticate(UserAuthenticationRequest request)
    {

        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var authenticatedUser = await authenticationProvider.AuthenticateAsync(request.Username, request.Password);
        if (authenticatedUser == null)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id.ToString()),
            new Claim(ClaimTypes.Name, authenticatedUser.Name),
            new Claim("UserId", authenticatedUser.Id.ToString())
        };

        if (authenticatedUser.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var token = JwtTokenFactory.GetJwtToken(
            authenticatedUser.Name,
            "ba9142eff388474c95811256682c2ea604a4902fa5c54e69a1d54d269f0ae3bd",
            "https://mysite.com",
            "https://mysite.com",
            TimeSpan.FromMinutes(60),
            claims.ToArray());

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expires = token.ValidTo
        });        
    }
}
