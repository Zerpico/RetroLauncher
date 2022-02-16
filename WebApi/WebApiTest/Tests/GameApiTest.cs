using Application.Features.Queries;
using Domain.Entities;
using MediatR;
using Moq;
using Persistence.Context;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Application.Shared;
using Microsoft.Extensions.Logging;
using static Application.Features.Queries.GetAllGamesQuery;
using static Application.Features.Queries.GetGamesByNameQuery;
using static Application.Features.Queries.GetGameByIdQuery;
using System;

namespace WebApiTest
{
    [Collection("Database collection")]
    public class GameApiTest 
    {       
        
        private readonly ApplicationDbContext _context;
        private readonly ILoggerFactory _loggerFactory;

        public GameApiTest(DatabaseFixture fixture)
        {
            _context = fixture.DbContext;
            _loggerFactory = fixture.LoggerFabrica;
        }

        [Fact]
        public async Task GameGetAllTestAsync()
        {  
            //Arange
            var mediator = new Mock<IMediator>();

            var _logger = _loggerFactory.CreateLogger<GetAllGamesQueryHandler>();

            GetAllGamesQuery command = new GetAllGamesQuery() { PageIndex = 1 };
            GetAllGamesQueryHandler handler = new GetAllGamesQueryHandler(_context, _logger) ;

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.NotNull(answer);
            Assert.IsAssignableFrom<PagingList<Game>>(answer);
            Assert.NotNull(answer.Items);
            Assert.IsAssignableFrom<IEnumerable<Game>>(answer.Items);
            Assert.Equal(3, answer.Items.Count());

        }

        [Fact]
        public async Task GameGetValidateTestAsync()
        {
            //Arange
            var mediator = new Mock<IMediator>();

            GetGameByIdQuery command = new GetGameByIdQuery() { Id = 1  };
            GetGameByIdQueryHandler handler = new GetGameByIdQueryHandler(_context);

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            var oneGame = answer;

            //Asert    
            Assert.NotNull(oneGame.Platform);
            Assert.NotNull(oneGame.GenreLinks);
            Assert.NotNull(oneGame.GameLinks);
            Assert.True(oneGame.GameLinks.Count > 0);
        }

        [Fact]
        public async Task GameGetFindByNameTestAsync()
        {
            //Arange
            var mediator = new Mock<IMediator>();

            GetGamesByNameQuery command = new GetGamesByNameQuery() { Name = "sonic" };
            GetGamesByNameQueryHandler handler = new GetGamesByNameQueryHandler(_context);

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            //Asert    
            Assert.NotNull(answer);
            Assert.NotNull(answer.Items);
            //Assert.True(answer.Items.Count() > 0);
        }
    }
}


    