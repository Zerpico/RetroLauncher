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
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Features.Queries.GetAllGenresQuery;

namespace WebApiTest
{
    public class GenreApiTest
    {        
        
        private readonly ApplicationDbContext _context;

     
        public GenreApiTest()
        {
            _context = TestDbContext.InitDbContext("testGenres");
        }

        [Fact]
        public async Task GenreGetAllTestAsync()
        {  

            //Arange
            var mediator = new Mock<IMediator>();

            GetAllGenresQuery command = new GetAllGenresQuery();
            GetAllGenresQueryHandler handler = new GetAllGenresQueryHandler(_context);

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.NotNull(answer);
            Assert.IsAssignableFrom<IEnumerable<Genre>>(answer);
            Assert.Equal(3, answer.Count());

        }
    }
}


    