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
using RetroLauncher.WebAPI.Controllers.Game;
using RetroLauncher.WebAPI.Controllers.Game.Get;

namespace RetroLauncher.WebAPI.Controllers
{   
    public class GameController : BaseApiController
    {
        private readonly ILogger<GameController> _logger;
        private readonly string _baseUrl;
        private readonly string _directoryRoms;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
            var url = Environment.GetEnvironmentVariable("BASEURL");
           // _baseUrl = url.EndsWith('/') ? url + "files/" : url + "/files/";
           // _directoryRoms = Path.Combine(Environment.GetEnvironmentVariable("ROMS_DIRECTORY"), "files");
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getList")]
        [ProducesResponseType(typeof(GamesGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetList([FromForm] GamesGetRequest request)
        {
            var resultQuery = await Mediator.Send(new GetAllGamesQuery { Name = request.Name, Genres = request.Genres, Platforms = request.Platforms, Limit = request.Limit > 100 ? 100 : request.Limit, Offset = request.Offset } );
            if (resultQuery.Items == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = "Not found items" });
            }

            var result = resultQuery.Items.Select(g => new Models.Game()
            {
                Id = g.Id,
                Name = g.Name,
                Alternative = g.Alternative,
                Year = g.Year,
                Publisher = g.Publisher,
                Genre = string.Join(", ", g.GenreLinks?.Select(x => x.Genre.Name)),
                Platform = new Models.Platform() { Name = g.Platform.Name, Alias = g.Platform.SmallName, Id = g.Platform.Id },
                Ratings = g.Rate
            });

            /*var result = resultQuery.Items.Select(g => new Models.Game()
            {
                Id = g.Id,
                Name = g.Name,
                NameSecond = g.NameSecond,
                NameOther = g.NameOther,
                Year = g.Year,
                Developer = g.Developer,
                Genre = g.Genre?.GenreName,
                Platform = new Models.Platform() { Id = g.Platform.Id,  PlatformName = g.Platform.PlatformName, Alias = g.Platform.Alias },
                GameLinks = (g.GameLinks != null && g.GameLinks.Count != 0 )? new List<Models.GameLink>()
                      { new Models.GameLink()
                            {
                                Url = _baseUrl + g.GameLinks?.Where(d => d.TypeUrl == Domain.Enums.TypeUrl.Cover).FirstOrDefault().Url.Replace('\\','/'),
                                TypeUrl = Models.TypeUrl.Cover
                            }
                      } : null,

                Ratings = g.Ratings != null ? (g.Ratings.Count == 0 ? 0 : Math.Round(g.Ratings.Average(d => d.RatingValue), 2)) : 0,
                Downloads = g.Downloads != null ? (g.Downloads.Count == 0 ? 0 : g.Downloads.Count) : 0
            });*/

            return Ok(new GamesGetResponse() { Items = result, Limit = resultQuery.Limit, Offset = resultQuery.Offset, Total = resultQuery.Total });
        }

        /// <summary>
        /// Gets Game by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getbyid")]
        [ProducesResponseType(typeof(GameGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromForm]GameGetRequest request)
        {
            var ans = await Mediator.Send(new GetGameByIdQuery { Id = request.Id });

            if (ans == null)
            {
                _logger.LogError($"Not found item by id {request.Id}");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = $"Not found item by id {request.Id}" });
            }

            var result = new Models.Game()
            {
                Id = ans.Id,
            };
            /*var result = new Models.Game()
            {
                Id = ans.Id,
                Name = ans.Name,
                NameSecond = ans.NameSecond,
                NameOther = ans.NameOther,
                Year = ans.Year,
                Developer = ans.Developer,
                Genre = ans.Genre?.GenreName,
                Platform = new Models.Platform() { Id = ans.Platform.Id, PlatformName = ans.Platform.PlatformName, Alias = ans.Platform.Alias },
                Annotation = ans.Annotation,
                GameLinks = ans.GameLinks.Select(l => new Models.GameLink()
                {
                    TypeUrl = (Models.TypeUrl)(int)l.TypeUrl,
                    Url = _baseUrl + l.Url.Replace('\\','/')
                }),  
                Ratings = ans.Ratings != null ? (ans.Ratings.Count == 0 ? 0 : Math.Round(ans.Ratings.Average(d => d.RatingValue), 2)) : 0, 
                Downloads = ans.Downloads != null ? (ans.Downloads.Count == 0 ? 0 : ans.Downloads.Count) : 0
            };*/

            return Ok(result);
        }


        /// <summary>
        /// Gets Game by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
/*        [HttpGet]
        [Route("getrom")]
        public async Task<IActionResult> GetRom(int id)
        {
            if (id <= 0)
                return BadRequest(new ErrorGetResponse() { ErrorMessage = $"For getRom request param 'id' must be gret than zero" });

            var ans = await Mediator.Send(new GetGameByIdQuery { Id = id });

            if (ans == null)
            {
                _logger.LogError($"Not found item by id {id}");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = $"Not found item by id {id}" });
            }

            if (ans.GameLinks != null && ans.GameLinks.Count != 0)
            {
                var romLink = ans.GameLinks.Where(g => g.TypeUrl == Domain.Enums.TypeUrl.Rom).FirstOrDefault();
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                    romLink.Url = romLink.Url.Replace('\\', '/');
                return File(System.IO.File.ReadAllBytes(System.IO.Path.Combine(_directoryRoms, romLink.Url)), "application/octet-stream", ans.Id + "_" + ans.Name.Replace(" ", "_") + ".7z");
            }
            else
            {
                _logger.LogError($"Not found item by id {id}");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = $"Not found item by id {id}" });
            }
        }
*/

    }
}
