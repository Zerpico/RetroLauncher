using Application.Interfaces;
using Application.Shared;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Queries
{
    public class GetAllGamesQuery : IRequest<PagingList<Game>>
    {
        public int PageIndex { get; set; }
        public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, PagingList<Game>>
        {
            private readonly IApplicationDbContext _context;
            private readonly ILogger<GetAllGamesQueryHandler> _logger;
            public GetAllGamesQueryHandler(IApplicationDbContext context, ILogger<GetAllGamesQueryHandler> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<PagingList<Game>> Handle(GetAllGamesQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var queryResult = _context.Games
                        .Include(x => x.Platform)
                        .Include(x => x.GenreLinks)
                            .ThenInclude(x => x.Genre)
                        .Include(x => x.GameLinks.OrderByDescending(l => l.Type))
                        .OrderByDescending(o => o.Rate);

                    var count = await queryResult.CountAsync();
                    var result = await queryResult
                        .Skip((query.PageIndex - 1) * 30)
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
                catch(Exception ex)
                {
                    _logger.LogError(ex, "GetAllGamesQueryHandler Error");
                    return null;
                }
            }
        }
    }
}
