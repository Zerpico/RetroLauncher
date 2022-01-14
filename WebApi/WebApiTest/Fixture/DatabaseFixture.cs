using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using System;
using Xunit;

namespace WebApiTest
{
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }
        public ILoggerFactory LoggerFabrica { get; private set; }
        public DatabaseFixture()
        {
            DbContext = TestDbContext.InitDbContext("testGames");

            LoggerFabrica = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
        }

        public void Dispose()
        {
            DbContext.Dispose();
            LoggerFabrica.Dispose();
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
