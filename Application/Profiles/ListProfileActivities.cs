using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListProfileActivities
    {
        public enum ProfileActivityFilter
        {
            FUTURE, PAST, HOST
        }

        public class Query : IRequest<Result<List<ProfileActivityDto>>>
        {
            public ProfileActivityFilter ActivityFilter { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ProfileActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this._mapper = mapper;
                this._context = context;
            }

            public async Task<Result<List<ProfileActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(a => a.Attendees.Any(at => at.AppUser.UserName == request.Username))
                    .OrderByDescending(a => a.Date)
                    .ProjectTo<ProfileActivityDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                query = request.ActivityFilter switch
                {
                    ProfileActivityFilter.FUTURE => query.Where(a => a.Date >= DateTime.UtcNow),
                    ProfileActivityFilter.PAST => query.Where(a => a.Date < DateTime.UtcNow),
                    _ => query.Where(a => a.HostUsername == request.Username)
                };

                return Result<List<ProfileActivityDto>>.Success(await query.ToListAsync());
            }
        }
    }
}