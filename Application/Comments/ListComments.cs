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

namespace Application.Comments
{
    public class ListComments
    {
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            public Guid ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            private readonly IMapper _mappper;
            private readonly DataContext _context;

            public Handler(DataContext context, IMapper mappper)
            {
                this._context = context;
                this._mappper = mappper;
            }

            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await _context.Comments
                    .Where(c => c.Activity.Id == request.ActivityId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ProjectTo<CommentDto>(_mappper.ConfigurationProvider)
                    .ToListAsync();
                
                return Result<List<CommentDto>>.Success(comments);
           }
        }
    }
}