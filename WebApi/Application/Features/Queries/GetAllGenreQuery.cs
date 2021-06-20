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
    public class GetAllGenresQuery : IRequest<IEnumerable<Genre>>
    {      

        public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<Genre>>
        {

            private readonly IApplicationDbContext _context;
            public GetAllGenresQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Genre>> Handle(GetAllGenresQuery query, CancellationToken cancellationToken)
            {
                return await _context.Genres.ToListAsync();
            }
          
        }
    }
}
