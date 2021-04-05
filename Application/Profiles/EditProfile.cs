using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class EditProfile
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.DisplayName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null) return Result<Unit>.NotFound();

                user.DisplayName = request.DisplayName;
                user.Bio = request.Bio;

                var success = await _context.SaveChangesAsync() > 0;
                return success ? Result<Unit>.Success() : Result<Unit>.Failure("Problem saving profile");
            }
        }
    }
}