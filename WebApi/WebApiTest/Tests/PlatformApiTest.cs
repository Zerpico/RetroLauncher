using Application.Features.Queries;
using Domain.Entities;
using MediatR;
using Moq;
using Persistence.Context;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Application.Features.Queries.GetAllPlatformQuery;

namespace WebApiTest
{
    [Collection("Database collection")]
    public class PlatformApiTest 
    {        
        
        private readonly ApplicationDbContext _context;
     
        public PlatformApiTest(DatabaseFixture fixture)
        {
            _context = fixture.DbContext;
        }

        [Fact]
        public async Task PlatformGetAllTestAsync()
        {  

            //Arange
            var mediator = new Mock<IMediator>();

            GetAllPlatformQuery command = new GetAllPlatformQuery();
            GetAllPlatformsQueryHandler handler = new GetAllPlatformsQueryHandler(_context);

            //Act
            var answer = await handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.NotNull(answer);
            Assert.IsAssignableFrom<IEnumerable<Platform>>(answer);
            Assert.Equal(3, answer.Count());

        }
    }
}


    