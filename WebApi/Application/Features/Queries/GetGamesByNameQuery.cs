using Application.Interfaces;
using Application.Shared;
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

    public class GetGamesByNameQuery : IRequest<PagingList<Game>>
    {
        public int PageIndex { get; set; }
        public string Name { get; set; }
        public int[] Platforms { get; set; }
        public int[] Genres { get; set; }

        public class GetGamesByNameQueryHandler : IRequestHandler<GetGamesByNameQuery, PagingList<Game>>
        {
            private readonly IApplicationDbContext _context;
            public GetGamesByNameQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<PagingList<Game>> Handle(GetGamesByNameQuery query, CancellationToken cancellationToken)
            {
                var queryResult = _context.Games
                    .Include(x => x.Platform)
                    .Include(x => x.GenreLinks)
                        .ThenInclude(x => x.Genre)
                    .Include(x => x.GameLinks.OrderByDescending(l => l.Type))
                    .Where(n => string.IsNullOrEmpty(query.Name) ? true : n.Name.ToLower().Contains(query.Name.ToLower()) || n.Alternative.ToLower().Contains(query.Name.ToLower()))
                    .Where(p => (query.Platforms == null || query.Platforms.Count() == 0) ? true : query.Platforms.Contains(p.Platform.Id))
                    .Where(p => (query.Genres == null || query.Genres.Count() == 0) ? true : p.GenreLinks.Any(g => query.Genres.Contains(g.GenreId)))
                    .OrderBy(o => o.Rate);

                var count = await queryResult.CountAsync();
                var result = await queryResult
                    .Skip((query.PageIndex-1) * 30)
                    .Take(30)
                    .ToListAsync(cancellationToken);

                int maxPage = (count / 30) + ((count % 30) > 0 ? 1 : 0);

                return new PagingList<Game>()
                {
                    Items = result,
                    Total = count,
                    Current = query.PageIndex,
                    Max = maxPage
                };
            }
        }
    }
}

