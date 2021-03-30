using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class DeletePhoto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly DataContext _context;

            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                this._context = context;
                this._photoAccessor = photoAccessor;
                this._userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(u => u.Photos)
                    .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null) return Result<Unit>.NotFound();

                var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);
                if (photo == null) return Result<Unit>.NotFound();
                if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

                try
                {
                    await _photoAccessor.DeletePhoto(photo.Id);
                    user.Photos.Remove(photo);

                    var success = await _context.SaveChangesAsync() > 0;
                    return success ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem deleting photo");
                }
                catch (System.Exception)
                {
                    return Result<Unit>.Failure("Problem deleting photo");
                }
            }
        }
    }
}