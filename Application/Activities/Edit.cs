using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : FluentValidation.AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this._mapper = mapper;
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null) return Result<Unit>.NotFound();

                _mapper.Map(request.Activity, activity);

                var success = await _context.SaveChangesAsync() > 0;
                if (success) {
                    return Result<Unit>.Success();
                } else {
                    return Result<Unit>.Failure("Error updating activity");
                }
            }
        }
    }
}