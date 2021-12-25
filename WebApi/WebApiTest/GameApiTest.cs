using Application.Features.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using System;
using System.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Features.Queries.GetGamesByNameQuery;
using Application.Shared;

namespace WebApiTest
{
    public class GameApiTest
    {        
        
        private readonly ApplicationDbContext _context;

       
        public GameApiTest()
        {
            _context = TestDbContext.InitDbContext("testGames");
        }

        [Fact]
        public async Task GameGetAllTestAsync()
        {  
            //Arange
            var mediator = new Mock<IMediator>();

            GetGamesByNameQuery command = new GetGamesByNameQuery() { Limit = 100, Offset = 0 };
            GetAllGamesQueryHandler handler = new GetAllGamesQueryHandler(_context) ;

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

            GetGamesByNameQuery command = new GetGamesByNameQuery() { Limit = 100, Offset = 0 };
            GetAllGamesQueryHandler handler = new GetAllGamesQueryHandler(_context);

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            var oneGame = answer.Items.FirstOrDefault();

            //Asert    
            Assert.NotNull(oneGame.Platform);
            Assert.NotNull(oneGame.GenreLinks);
            //Assert.NotNull(oneGame.GameLinks);
            //Assert.True(oneGame.GameLinks.Count > 0);
        }

        [Fact]
        public async Task GameGetFindByNameTestAsync()
        {
            //Arange
            var mediator = new Mock<IMediator>();

            GetGamesByNameQuery command = new GetGamesByNameQuery() { Limit = 100, Offset = 0, Name = "sonic" };
            GetAllGamesQueryHandler handler = new GetAllGamesQueryHandler(_context);

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            //Asert    
            Assert.NotNull(answer);
            Assert.NotNull(answer.Items);
            Assert.True(answer.Items.Count() > 0);
        }
    }
}


    