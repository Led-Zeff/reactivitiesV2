using System;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IMediator mediator, ILogger<ChatHub> logger)
        {
            this._logger = logger;
            this._mediator = mediator;
        }

        public async Task SendComment(CreateComment.Command command)
        {
            var comment = await _mediator.Send(command);

            if (comment.Type != Application.Core.ResultType.SUCCESS) {
                throw new Exception(comment.Error);
            }

            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var comments = await _mediator.Send(new ListComments.Query { ActivityId = Guid.Parse(activityId) });
            await Clients.Caller.SendAsync("LoadComments", comments.Value);
        }
    }
}