using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourcesManagementApi.ApiModel;
using ResourcesManagementApi.Application.CreateResource;
using ResourcesManagementApi.Application.LockResource;
using ResourcesManagementApi.Application.UnlockResource;
using ResourcesManagementApi.Application.WithdrawnResource;

namespace ResourcesManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly ISender sender;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration configuration;

    public ResourcesController(ISender sender, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateResource()
    {
        var resourceId = await this.sender.Send(new CreateResourceCommand());

        var createdResource = new { Id = resourceId };
        var routeValues = new { id = createdResource.Id };

        return CreatedAtRoute("GetResourceRoute", routeValues, createdResource);
    }

    // just to be able to return 201 in case above
    [Authorize]
    [HttpGet("{id}", Name = "GetResourceRoute")]
    public IActionResult GetResource()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/withdrawn")]
    public async Task<IActionResult> WithdrawnResource([FromRoute]int id)
    {
        await this.sender.Send(new WithdrawnResourceCommand(id));

        return Ok();
    }

    [Authorize]
    [HttpPatch("{id}/lock")]
    public async Task<IActionResult> LockResource([FromRoute]int id, [FromBody] LockResourceRequest request)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest();
        }

        var requestingUserId = httpContextAccessor.ParseUserId();
        if (requestingUserId.HasValue == false)
        {
            return Forbid();
        }

        await this.sender.Send(new LockResourceCommand(id, requestingUserId.Value, request.LockKind == ResourceLockKind.Permanent));

        return Ok();
    }

    [Authorize]
    [HttpPatch("{id}/unlock")]
    public async Task<IActionResult> UnlockResource([FromRoute] int id)
    {
        var requestingUserId = httpContextAccessor.ParseUserId();
        if (requestingUserId.HasValue == false)
        {
            return Forbid();
        }

        await this.sender.Send(new UnlockResourceCommand(id, requestingUserId.Value));

        return Ok();
    }
}
