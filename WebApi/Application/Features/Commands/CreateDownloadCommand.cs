using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace RetroLauncher.Application.Features.Commands
{
    public class CreateDownloadCommand : IRequest<int>
    {
        public Game Game { get; set; }
        public class CreateDownloadCommandHandler : IRequestHandler<CreateDownloadCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateDownloadCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateDownloadCommand command, CancellationToken cancellationToken)
            {
                var download = new Download();
                download.Game = command.Game;
                download.Dt = DateTime.UtcNow;

                _context.Downloads.Add(download);
                await _context.SaveChangesAsync();
                return download.Id;
            }
        }
    }
}
