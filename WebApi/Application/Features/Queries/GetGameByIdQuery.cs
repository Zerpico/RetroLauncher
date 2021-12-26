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
    public class GetGameByIdQuery : IRequest<Game>
    {
        public int Id { get; set; }        

        public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, Game>
        {
            private readonly IApplicationDbContext _context;
            public GetGameByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Game> Handle(GetGameByIdQuery query, CancellationToken cancellationToken)
            {
                var queryResult = await _context.Games
                    .Include(x => x.Platform)
                    .Include(x => x.GenreLinks)
                        .ThenInclude(x => x.Genre)
                    .Include(x => x.GameLinks.OrderByDescending(l => l.Type))
                    .Where(n => n.Id == query.Id)
                    .FirstOrDefaultAsync();

                return queryResult;
            }
        }
    }
}
