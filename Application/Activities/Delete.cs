using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null) {
                    return Result<Unit>.NotFound();
                }

                _context.Remove(activity);
                var success = await _context.SaveChangesAsync() > 0;
                if (success) {
                    return Result<Unit>.Success();
                } else {
                    return Result<Unit>.Failure("Error deleting activity");
                }
            }
        }
    }
}