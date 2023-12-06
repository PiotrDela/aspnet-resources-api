using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourcesManagementApi.Authentication;
using ResourcesManagementApi.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ResourcesManagementApi.Controllers;

[ApiController]
[Route("[controller]")]
public partial class AuthenticationController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("token")]
    public IActionResult Authenticate(UserAuthenticationRequest loginUser)
    {

        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var user = new User();
        user.Name = "John";
        user.IsAdmin = true;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("Id", user.Id.ToString())
        };

        if (user.IsAdmin)
        {
            claims.Add(new Claim("Role", "Admin"));
        }

        // create a new token with token helper and add our claim
        // from `Westwind.AspNetCore`  NuGet Package
        var token = JwtHelper.GetJwtToken(
            loginUser.Username,
            "00VVxkE6lvrgSP4gZykwocNua4reiymQ/KtPRLGBZRn/FOwyg/WZeisj/YruV1Pd",
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
