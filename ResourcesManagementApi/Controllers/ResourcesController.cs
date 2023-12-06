using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResourcesManagementApi.Application.Commands;

namespace ResourcesManagementApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly ISender sender;

    public ResourcesController(ISender sender)
    {
        this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    [Route("/")]
    [HttpPost]
    public async Task<IActionResult> CreateResource()
    {
        var resourceId = await this.sender.Send(new CreateResourceCommand());
        return Ok(resourceId);
    }

    [Route("/{id}/withdrawn")]
    [HttpPatch]
    public async Task<IActionResult> WithdrawnResource(int id)
    {
        try
        {
            await this.sender.Send(new ModifyResourceCommand(id, (resource) => { resource.Withdrawn(); }));
        }
        catch(Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok();
    }

    [Route("/{id}/lock")]
    [HttpPatch]
    public async Task<IActionResult> LockResource(int id)
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
}
