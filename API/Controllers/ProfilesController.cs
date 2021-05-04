using System.Threading.Tasks;
using Application.Profiles;
using Microsoft.AspNetCore.Mvc;
using static Application.Profiles.ListProfileActivities;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new ProfileDetails.Query{ Username = username }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(EditProfile.Command update)
        {
            return HandleResult(await Mediator.Send(update));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetProfileActivities(string username, [FromQuery] ProfileActivityFilter filter) {
            return HandleResult(await Mediator.Send(new ListProfileActivities.Query{ Username = username, ActivityFilter = filter }));
        }
    }
}