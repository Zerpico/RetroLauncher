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
    public class GetAllGamesQuery : IRequest<IEnumerable<Game>>
    {
        public string Name { get; set; }
        public int[] Genres { get; set; }
        public int[] platforms { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }

        public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, IEnumerable<Game>>
        {

            private readonly IApplicationDbContext _context;
            public GetAllGamesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Game>> Handle(GetAllGamesQuery query, CancellationToken cancellationToken)
            {
                var result = await _context.Games
                    .Include(x => x.Genre)
                    .Include(x => x.Platform)
                    .Include(x => x.GameLinks)
                    .Include(x => x.Ratings)
                    .Include(x => x.Downloads)
                    .Take(query.limit)
                    .Skip(query.offset)
                    .ToListAsync();

                if (result == null)
                {
                    return null;
                }
                return result;
            }
        }
    }
}
