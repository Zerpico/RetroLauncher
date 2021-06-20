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
    public class GetAllGamesQuery : IRequest<PagingList<Game>>
    {
        public string Name { get; set; }
        public int[] Genres { get; set; }
        public int[] Platforms { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

        public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, PagingList<Game>>
        {

            private readonly IApplicationDbContext _context;
            public GetAllGamesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<PagingList<Game>> Handle(GetAllGamesQuery query, CancellationToken cancellationToken)
            {
                query.Genres = query.Genres ?? (new int[0]);
                query.Platforms = query.Platforms ?? (new int[0]);

                var queryResult = _context.Games
                    .Include(x => x.Genre)
                    .Include(x => x.Platform)
                    .Include(x => x.GameLinks)
                    .Include(x => x.Ratings)
                    .Include(x => x.Downloads)
                    .Where(n => string.IsNullOrEmpty(query.Name) ? true : n.Name.Contains(query.Name) || n.NameSecond.Contains(query.Name) || n.NameOther.Contains(query.Name))
                    .Where(g => query.Genres.Count() == 0 ? true : query.Genres.Contains(g.Genre.Id))
                    .Where(p => query.Platforms.Count() == 0 ? true : query.Platforms.Contains(p.Platform.Id));

                int count = await queryResult.CountAsync();
                
                return
                new PagingList<Game>()
                {
                    Total = count,
                    Offset = query.Offset,
                    Limit = query.Limit,
                    Items = await queryResult.Skip(query.Offset).Take(query.Limit).ToListAsync()
                };
            }

           
        }
    }
}
