using Microsoft.EntityFrameworkCore;
using RetroLauncher.DAL.Model;
using RetroLauncher.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebApi.Service
{
    public class SQLRepository : IDataRepository
    {
        DbLibraryGamesContext _repository;
        public SQLRepository(DbLibraryGamesContext repository)
        {
            _repository = repository;
        }
        //TODO: переделать всё это 
        public PagingGames GetGames(string name, int[] genres,  int[] platforms, int limit = 50, int offset = 0)
        {

            var query = _repository.Games
                      .Include(x => x.Genre)
                      .Include(d => d.Platform)
                      ?.Where(n => string.IsNullOrEmpty(name) ? true : n.GameName.Contains(name) || n.NameSecond.Contains(name) || n.NameOther.Contains(name))
                      ?.Where(g => genres.Count() == 0 ? true : genres.Contains(g.GenreId))
                      ?.Where(p => platforms.Count() == 0 ? true : platforms.Contains(p.PlatformId))
                      ?.Select(g => new RetroLauncher.DAL.Model.Game()
                      {
                          GameId = g.GameId,
                          Name = g.GameName,
                          NameSecond = g.NameSecond,
                          NameOther = g.NameOther,
                          Year = g.Year,
                          Developer = g.Developer,
                          Genre = new DAL.Model.Genre(g.Genre.GenreId, g.Genre.GenreName),
                          Platform = new DAL.Model.Platform() { PlatformId = g.Platform.PlatformId, PlatformName = g.Platform.PlatformName, Alias = g.Platform.Alias },
                          GameLinks = new List<DAL.Model.GameLink>()
                          { new DAL.Model.GameLink()
                            {
                                LinkId = g.GameLinks.Where(d => d.TypeUrl == 2).FirstOrDefault().LinkId,
                                Url = "https://www.zerpico.ru/retrolauncher/"+g.GameLinks.Where(d => d.TypeUrl == 2).FirstOrDefault().Url.Replace('\\','/'),
                                TypeUrl = (TypeUrl)g.GameLinks.Where(d => d.TypeUrl == 2).FirstOrDefault().TypeUrl
                            }
                          },
                          Rating = g.Ratings.Count() == 0 ? null : new Nullable<double>(Math.Round(g.Ratings.Average(d => d.RatingValue), 2)),
                          Downloads = g.Downloads.Count() == 0 ? null : new Nullable<int>(g.Downloads.Count)

                      });



            int count = query.Count();
            var items = query.Skip(offset).Take(limit);
            return 
                new PagingGames()
                {
                    Total = count,
                    Offset = offset,
                    Limit = limit,
                    Items = items.ToList()
                };
        }


        public DAL.Model.Game GetGameById(int id)
        {
            //ищем игру по id
            var selGame =  _repository.Games
                            .Include(x => x.Genre)
                            .Include(d => d.Platform)
                            .Include(l => l.GameLinks)
                            .Include(r => r.Ratings)
                            .Include(dd => dd.Downloads)
                            .Where(g => g.GameId == id).FirstOrDefault();

            if (selGame == null) return null;

            //создаем список ссылок
            var links = new List<DAL.Model.GameLink>();
            foreach (var lnk in selGame.GameLinks)
                links.Add(new DAL.Model.GameLink()
                {
                    LinkId = lnk.LinkId,
                    Url = "https://www.zerpico.ru/retrolauncher/" + lnk.Url.Replace('\\', '/'),
                    TypeUrl = (DAL.Model.TypeUrl)lnk.TypeUrl
                });

            //составляем результат
            var result = new DAL.Model.Game()
            {
                GameId = selGame.GameId,
                Name = selGame.GameName,
                NameSecond = selGame.NameSecond,
                NameOther = selGame.NameOther,
                Year = selGame.Year,
                Developer = selGame.Developer,
                Annotation = selGame.Annotation,
                Genre = new DAL.Model.Genre(selGame.Genre.GenreId, selGame.Genre.GenreName),
                Platform = new DAL.Model.Platform() { PlatformId = selGame.Platform.PlatformId, PlatformName = selGame.Platform.PlatformName, Alias = selGame.Platform.Alias },
                GameLinks = links,
                Rating = selGame.Ratings.Count() == 0 ? null : new Nullable<double>(Math.Round(selGame.Ratings.Average(d => d.RatingValue), 2)),
                Downloads = selGame.Downloads.Count() == 0 ? null : new Nullable<int>(selGame.Downloads.Count)
            };

            return result;
        }


        public List<DAL.Model.Genre> GetGenres()
        {            
            var query =  _repository.Genres.Select(g => new RetroLauncher.DAL.Model.Genre(g.GenreId, g.GenreName)).ToList();

            return query;
        }

        public List<DAL.Model.Platform> GetPlatforms()
        {
            var query =  _repository.Platforms.Select(pl => new RetroLauncher.DAL.Model.Platform(pl.PlatformId, pl.PlatformName, pl.Alias)).ToList();

            return query;
        }
    }
}
