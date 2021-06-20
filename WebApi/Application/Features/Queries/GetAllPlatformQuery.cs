using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Queries
{
    public class GetAllPlatformQuery : IRequest<IEnumerable<Platform>>
    {
        public class GetAllPlatformsQueryHandler : IRequestHandler<GetAllPlatformQuery, IEnumerable<Platform>>
        {

            private readonly IApplicationDbContext _context;
            public GetAllPlatformsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Platform>> Handle(GetAllPlatformQuery query, CancellationToken cancellationToken)
            {
                return await _context.Platforms.ToListAsync();
            }

        }
    }

    
}
