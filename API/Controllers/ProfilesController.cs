using System.Threading.Tasks;
using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new ProfileDetails.Query{ Username = username }));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProfile(EditProfile.Command update)
        {
            return HandleResult(await Mediator.Send(update));
        }
    }
}