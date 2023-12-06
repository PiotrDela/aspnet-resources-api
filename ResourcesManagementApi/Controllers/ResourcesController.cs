using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourcesManagementApi.Application.Commands;
using System.ComponentModel.DataAnnotations;

namespace ResourcesManagementApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly ISender sender;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ResourcesController(ISender sender, IHttpContextAccessor httpContextAccessor)
    {
        this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    //[Authorize(Roles = "Admin")]
    [Route("/")]
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
    [Route("/{id}")]
    [HttpGet(Name = "GetResourceRoute")]
    public IActionResult GetResource()
    {
        throw new NotImplementedException();
    }

    //[Authorize(Roles = "Admin")]
    [Route("/{id}/withdrawn")]
    [HttpPatch]
    public async Task<IActionResult> WithdrawnResource([FromRoute]int id)
    {
        try
        {
            await this.sender.Send(new ModifyResourceCommand(id, (resource) => { resource.Withdrawn(); }));
        }
        catch (Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok();
    }

    //[Authorize]
    [Route("/{id}/lock")]
    [HttpPatch]
    public async Task<IActionResult> LockResource([FromRoute]int id, [FromBody] LockResourceRequest request)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest();
        }

        var lockPeriod = request.LockKind == ResourceLockKind.Temporary ? TimeSpan.FromDays(1) : TimeSpan.MaxValue; // TODO: fetch temporary period from config
        var user = new Domain.Entities.User() { Id = 1 }; // TODO: fetch it from http context using httpcontext accessort
        try
        {
            await this.sender.Send(new ModifyResourceCommand(id, (resource) => { resource.LockBy(user, lockPeriod); }));
        }
        catch (Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Domain.Exceptions.BusinessRuleValidationException ex)
        {
            return Forbid();
        }

        return Ok();
    }

    public enum ResourceLockKind
    {
        Temporary = 0,
        Permanent = 1,
    }

    public class LockResourceRequest
    {
        [Required]
        public ResourceLockKind? LockKind { get; set; }
    }
}
