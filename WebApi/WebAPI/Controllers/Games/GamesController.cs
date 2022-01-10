using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Queries;
using Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RetroLauncher.WebAPI.Controllers.Games.Dto;
using RetroLauncher.WebAPI.Controllers.Games.Get;

namespace RetroLauncher.WebAPI.Controllers
{   
    public class GamesController : BaseApiController
    {
        private readonly ILogger<GamesController> _logger;
        private readonly string _baseUrl;
        private readonly string _directoryRoms;

        public GamesController(ILogger<GamesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _baseUrl = configuration["BaseUrl"];
        }


        /// <summary> Fetch games list </summary>
        /// <param name="request">request for fetch</param>
        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(typeof(GamesGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] GamesGetRequest request)
        {
            var resultQuery = await Mediator.Send(new GetAllGamesQuery() { PageIndex = request.Page });
            if (resultQuery == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { Code = 400, Status = "Not found items" });
            }

            var fields = request.Fields.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s=>s.Trim().ToLower());                       

            var result = resultQuery.Items
                .Select(g => new Game()
                {
                    Id = g.Id,
                    Name = g.Name.Trim(),
                    Alternative = g.Alternative.Trim(),
                    Annotation = fields.Any(s => s == nameof(g.Annotation).ToLower()) ? g.Annotation : string.Empty,
                    Platform = g.Platform.Id,
                    Publisher = fields.Any(s=> s == nameof(g.Publisher).ToLower()) ? g.Publisher : string.Empty,
                    Year = g.Year,
                    Ratings = g.Rate.HasValue ? Math.Round(g.Rate.Value, 2) : g.Rate,
                    Genres = g.GenreLinks?.Select(s=>s.GenreId).ToList(),
                    Links = GetLinksWithCover(g)
                });

            _logger.LogInformation("Games GetList", request.Page, request.Fields);

            return Ok(new GamesGetResponse()
            {
                Pages = new Pages() { Current = resultQuery.Current, Max = resultQuery.Max },
                Data = new GameData() { Games = result.ToList(), Count = result.Count() }
            });            
        }

       
        /// <summary> Fetch games list </summary>
        /// <param name="request">request for fetch</param>
        [HttpGet]
        [Route("GetByName")]
        [ProducesResponseType(typeof(GamesGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByName([FromQuery] GamesGetByNameRequest request)
        {
            var resultQuery = await Mediator.Send(new GetGamesByNameQuery() { PageIndex = request.Page, Name = request.Name, Genres = request.Genres, Platforms = request.Platforms});
            if (resultQuery == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { Code = 400, Status = "Not found items" });
            }

            var fields = request.Fields.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLower());

            var result = resultQuery.Items
                .Select(g => new Game()
                {
                    Id = g.Id,
                    Name = g.Name.Trim(),
                    Alternative = g.Alternative.Trim(),
                    Annotation = fields.Any(s => s == nameof(g.Annotation).ToLower()) ? g.Annotation : string.Empty,
                    Platform = g.Platform.Id,
                    Publisher = fields.Any(s => s == nameof(g.Publisher).ToLower()) ? g.Publisher : string.Empty,
                    Year = g.Year,
                    Ratings = g.Rate.HasValue ? Math.Round(g.Rate.Value, 2) : g.Rate,
                    Genres = g.GenreLinks?.Select(s => s.GenreId).ToList(),
                    Links = GetLinksWithCover(g)
                });

            _logger.LogInformation("Games GetByName", request.Page, request.Fields, request.Name, request.Genres, request.Platforms);

            return Ok(new GamesGetResponse()
            {
                Pages = new Pages() { Current = resultQuery.Current, Max = resultQuery.Max },
                Data = new GameData() { Games = result.ToList(), Count = result.Count() }
            });
        }



        /// <summary> Fetch games by id </summary>
        /// <param name="request">request for fetch</param>
        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(typeof(GameGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromQuery] GameGetByIdRequest request)
        {
            var resultQuery = await Mediator.Send(new GetGameByIdQuery() { Id = request.Id });
            if (resultQuery == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { Code = 400, Status = "Not found items" });
            }

            var result = new List<Game>()
            { new Game()
                {
                    Id = resultQuery.Id,
                    Name = resultQuery.Name,
                    Alternative = resultQuery.Alternative,
                    Annotation = resultQuery.Annotation,
                    Platform = resultQuery.Platform.Id,
                    Publisher = resultQuery.Publisher,
                    Year = resultQuery.Year,
                    Ratings = resultQuery.Rate.HasValue ? Math.Round(resultQuery.Rate.Value, 2) : resultQuery.Rate,
                    Genres = resultQuery.GenreLinks?.Select(s => s.GenreId).ToList(),
                    Links = GetLinks(resultQuery)
                }
            };

            _logger.LogInformation("Games GetById", request.Id);

            return Ok(new GameGetResponse()
            {
                Data = new GameData() { Games = result, Count = 1 }
            });
        }
        

        private ICollection<GameLink> GetLinksWithCover(Domain.Entities.Game game)
        {
            var nameDir = game.Name.Trim().Replace("'", "").Replace(':', ' ').Replace('\\', '_').Replace('/', '_').Replace('?', ' ').Replace('<', '_').Replace('>', '_').Replace(' ','_');
           
            var flink = game.GameLinks.FirstOrDefault();
            return new List<GameLink>() { new GameLink()
                {
                    Type = "cover",
                    Url = new Uri(new Uri(_baseUrl), $"IMAGES/{game.Platform.SmallName}/{nameDir}/{flink.Url}").ToString(),
                }
            };
        }

        private ICollection<GameLink> GetLinks(Domain.Entities.Game game)
        {
            var gameLinks = game.GameLinks.ToList();
            gameLinks[0].Type = Domain.Enums.TypeUrl.Cover;
            
            var nameDir = game.Name.Trim().Replace("'", "").Replace(':', ' ').Replace('\\', '_').Replace('/', '_').Replace('?', ' ').Replace('<', '_').Replace('>', '_').Replace(' ', '_');
           
            return gameLinks?.Select(s => new GameLink()
            {
                Type = s.Type switch
                {
                    Domain.Enums.TypeUrl.Rom => "rom" ,
                    Domain.Enums.TypeUrl.Screen => "screen",
                    Domain.Enums.TypeUrl.Cover => "cover",
                    Domain.Enums.TypeUrl.CoverBack => "cover",
                },
                Url = s.Type switch
                {
                    Domain.Enums.TypeUrl.Rom => new Uri(new Uri(_baseUrl), $"ROMS/{game.Platform.SmallName}/{s.Url}").ToString(),
                    _ => new Uri(new Uri(_baseUrl), $"IMAGES/{game.Platform.SmallName}/{nameDir}/{s.Url}").ToString(),
                }
            }).ToList();
        }

    }
}
