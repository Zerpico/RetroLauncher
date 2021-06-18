using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetroLauncher.WebAPI.Controllers.v1.Game;
using RetroLauncher.WebAPI.Controllers.v1.Game.Get;

namespace RetroLauncher.WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class GameController : BaseApiController
    {
        private readonly ILogger<GameController> _logger;
        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GamesGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(GamesGetRequest request)
        {
            var games = await Mediator.Send(new GetAllGamesQuery { limit = request.Limit, offset = request.Offset } );
            if (games == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = "Not found items" });
            }

            var result = games.Select(g => new Models.Game()
            {
                Id = g.Id,
                Name = g.Name,
                NameSecond = g.NameSecond,
                NameOther = g.NameOther,
                Year = g.Year,
                Developer = g.Developer,
                Genre = g.Genre?.GenreName,
                Platform = g.Platform?.PlatformName,
                GameLinks = g.GameLinks != null ? new List<Models.GameLink>()
                      { new Models.GameLink()
                            {
                                Url = g.GameLinks?.Where(d => d.TypeUrl == Domain.Enums.TypeUrl.Cover).FirstOrDefault().Url.Replace('\\','/'),
                                TypeUrl = Models.TypeUrl.Cover
                            }
                      } : null,

                Ratings = g.Ratings != null ? (g.Ratings.Count() == 0 ? 0 : Math.Round(g.Ratings.Average(d => d.RatingValue), 2)) : 0,
                Downloads = g.Downloads != null ? (g.Downloads.Count() == 0 ? 0 : g.Downloads.Count) : 0
            });

            return Ok(new GamesGetResponse() { Items = result, Limit = 50, Offset = 0, Total = 1 });
        }

        /// <summary>
        /// Gets Game by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var ans = await Mediator.Send(new GetGameByIdQuery { Id = id });

            if (ans == null)
            {
                _logger.LogError($"Not found item by id {id}");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = $"Not found item by id {id}" });
            }

            var result = new Models.Game()
            {
                Id = ans.Id,
                Name = ans.Name,
                NameSecond = ans.NameSecond,
                NameOther = ans.NameOther,
                Year = ans.Year,
                Developer = ans.Developer,
                Genre = ans.Genre?.GenreName,
                Platform = ans.Platform?.PlatformName,
                Annotation = ans.Annotation,
                GameLinks = ans.GameLinks.Select(l => new Models.GameLink()
                {
                    TypeUrl = (Models.TypeUrl)(int)l.TypeUrl,
                    Url = l.Url.Replace('\\','/')
                }),  
                Ratings = ans.Ratings != null ? (ans.Ratings.Count() == 0 ? 0 : Math.Round(ans.Ratings.Average(d => d.RatingValue), 2)) : 0, 
                Downloads = ans.Downloads != null ? (ans.Downloads.Count() == 0 ? 0 : ans.Downloads.Count) : 0
            };

            return Ok(result);
        }
    }
}
