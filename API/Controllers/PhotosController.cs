using System.Threading.Tasks;
using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddPhoto.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new DeletePhoto.Command{ Id = id }));
        }

        [HttpPut("{id}/setMain")]
        public async Task<ActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command{ Id = id }));
        }
    }
}