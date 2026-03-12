using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private IMediator? _mediator;

    // Eğer _mediator null ise HttpContext üzerinden çek, değilse mevcut olanı kullan.
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}