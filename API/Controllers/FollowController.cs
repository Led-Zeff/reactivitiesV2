using System.Threading.Tasks;
using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command{TargetUserName = username}));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowers(string username, ListFollowers.ListPredicate predicate)
        {
            return HandleResult(await Mediator.Send(new ListFollowers.Query{ Username = username, Predicate = predicate }));
        }
    }
}