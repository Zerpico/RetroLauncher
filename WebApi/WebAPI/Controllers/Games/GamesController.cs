using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Queries;
using Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
            var url = Environment.GetEnvironmentVariable("BASEURL");
           // _baseUrl = url.EndsWith('/') ? url + "files/" : url + "/files/";
           // _directoryRoms = Path.Combine(Environment.GetEnvironmentVariable("ROMS_DIRECTORY"), "files");
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
                    Name = g.Name,
                    Alternative = g.Alternative,
                    Annotation = fields.Any(s => s == nameof(g.Annotation).ToLower()) ? g.Annotation : string.Empty,
                    Platform = g.Platform.Id,
                    Publisher = fields.Any(s=> s == nameof(g.Publisher).ToLower()) ? g.Publisher : string.Empty,
                    Year = g.Year,
                    Ratings = g.Rate.HasValue ? Math.Round(g.Rate.Value, 2) : g.Rate,
                    Genres = g.GenreLinks?.Select(s=>s.GenreId).ToList(),
                    Links = GetLinksWithCover(g.GameLinks)
                });

            

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
                    Name = g.Name,
                    Alternative = g.Alternative,
                    Annotation = fields.Any(s => s == nameof(g.Annotation).ToLower()) ? g.Annotation : string.Empty,
                    Platform = g.Platform.Id,
                    Publisher = fields.Any(s => s == nameof(g.Publisher).ToLower()) ? g.Publisher : string.Empty,
                    Year = g.Year,
                    Ratings = g.Rate.HasValue ? Math.Round(g.Rate.Value, 2) : g.Rate,
                    Genres = g.GenreLinks?.Select(s => s.GenreId).ToList(),
                    Links = GetLinksWithCover(g.GameLinks)
                });

           
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
        [ProducesResponseType(typeof(GamesGetResponse), StatusCodes.Status200OK)]
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
                    Links = GetLinks(resultQuery.GameLinks.ToList())
                }
            };


            return Ok(new GamesGetResponse()
            {
                Pages = new Pages() { Current = 1, Max = 1 },
                Data = new GameData() { Games = result, Count = 1 }
            });
        }


        private ICollection<GameLink> GetLinksWithCover(ICollection<Domain.Entities.GameLink> gameLinks)
        {
            var fgame = gameLinks.FirstOrDefault();
            return new List<GameLink>() { new GameLink()
                {
                    Type = "cover",
                    Url = fgame.Url
                }
            };
        }

        private ICollection<GameLink> GetLinks(List<Domain.Entities.GameLink> gameLinks)
        {
            gameLinks[0].Type = Domain.Enums.TypeUrl.Cover;

            return gameLinks?.Select(s => new GameLink()
            {
                Type = s.Type switch
                {
                    Domain.Enums.TypeUrl.Rom => "rom" ,
                    Domain.Enums.TypeUrl.Screen => "screen",
                    Domain.Enums.TypeUrl.Cover => "cover",
                    Domain.Enums.TypeUrl.CoverBack => "cover",
                },
                Url = s.Url
            }).ToList();
        }

    }
}
