using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result) {
            switch (result.Type)
            {
                case ResultType.SUCCESS:
                    return Ok(result.Value);
                case ResultType.NOT_FOUND:
                    return NotFound();
                default:
                    return BadRequest(result.Error);
            }
        }
    }
}