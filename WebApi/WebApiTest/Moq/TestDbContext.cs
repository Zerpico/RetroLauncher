using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTest
{
    public class TestDbContext
    {
        public static ApplicationDbContext InitDbContext()            
        {
            return InitDbContext("testDatabase");
        }

        public static ApplicationDbContext InitDbContext(string databasename)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databasename)
            .Options;

            // Insert seed data into the database using one instance of the context
            ApplicationDbContext context = new ApplicationDbContext(options);

            //genres
            context.Genres.AddRange(
                new Genre() { Id = 1, Name = "Платформер" },
                new Genre() { Id = 2, Name = "Аркада" },
                new Genre() { Id = 3, Name = "Гонки" });

            //Platformers
            context.Platforms.AddRange(
                new Platform() { Id = 1, Name = "NES", SmallName = "nes" },
                new Platform() { Id = 2, Name = "SMS", SmallName = "sms" },
                new Platform() { Id = 3, Name = "SMG", SmallName = "smg" });
            context.SaveChanges();

            //Games
            var game1 = new Game()
            {
                Id = 1,
                Name = "Sonic The Hedgehog",
                Alternative = "ソニック・ザ・ヘッジホッグ",
                GenreLinks = new List<GenreLink>(),
                Platform = context.Platforms.Where(g => g.Id == 3).FirstOrDefault(),
                Annotation = "sample text",
                Year = "1991"
            };

            var gamelink1 = new GameLink() { Id = 1, Type = Domain.Enums.TypeUrl.Cover, Url = "someUrl", Game = game1 };
            var gamelink2 = new GameLink() { Id = 2, Type = Domain.Enums.TypeUrl.Rom, Url = "someUrl", Game = game1 };
            var gamelink3 = new GameLink() { Id = 3, Type = Domain.Enums.TypeUrl.Screen, Url = "someUrl", Game = game1 };
            game1.GameLinks = new List<GameLink>() { gamelink1, gamelink2, gamelink3 };

            var game2 = new Game()
            {
                Id = 2,
                Name = "Sonic The Hedgehog 2",
                //Genre = context.Genres.Where(g => g.Id == 2).FirstOrDefault(),
                Platform = context.Platforms.Where(g => g.Id == 3).FirstOrDefault(),
                Annotation = "sample text",
                Year = "1992"
            };
            var gamelink4 = new GameLink() { Id = 4, Type = Domain.Enums.TypeUrl.Cover, Url = "someUrl", Game = game2 };
            var gamelink5 = new GameLink() { Id = 5, Type = Domain.Enums.TypeUrl.Rom, Url = "someUrl", Game = game2 };
            var gamelink6 = new GameLink() { Id = 6, Type = Domain.Enums.TypeUrl.Screen, Url = "someUrl", Game = game2 };
            game2.GameLinks = new List<GameLink>() { gamelink4, gamelink5, gamelink6 };

            var game3 = new Game()
            {
                Id = 3,
                Name = "Sonic The Hedgehog 3",
                //Genre = context.Genres.Where(g => g.Id == 2).FirstOrDefault(),
                Platform = context.Platforms.Where(g => g.Id == 3).FirstOrDefault(),
                Annotation = "sample text",
                Year = "1993"
            };
            var gamelink7 = new GameLink() { Id = 7, Type = Domain.Enums.TypeUrl.Cover, Url = "someUrl", Game = game3 };
            var gamelink8 = new GameLink() { Id = 8, Type = Domain.Enums.TypeUrl.Rom, Url = "someUrl", Game = game3 };
            var gamelink9 = new GameLink() { Id = 9, Type = Domain.Enums.TypeUrl.Screen, Url = "someUrl", Game = game3 };
            game3.GameLinks = new List<GameLink>() { gamelink4, gamelink5, gamelink6 };
            context.Games.AddRange(game1, game2, game3);

            //save
            context.SaveChanges();
            return context;
        }
    }
}
