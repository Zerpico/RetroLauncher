using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Queries;
using Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RetroLauncher.Application.Features.Commands;
using RetroLauncher.WebAPI.Controllers.Games.Dto;
using RetroLauncher.WebAPI.Controllers.Games.Get;

namespace RetroLauncher.WebAPI.Controllers
{       
    public class GamesController : BaseApiController
    {
        private readonly ILogger<GamesController> _logger;
        private readonly string _baseUrl;
        private readonly string _filesPath;

        public GamesController(ILogger<GamesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _baseUrl = configuration["BaseUrl"];
            _filesPath = configuration["FilesPath"];
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
                    Name = string.IsNullOrEmpty(g.Name) ? string.Empty : g.Name.Trim(),
                    Alternative = string.IsNullOrEmpty(g.Alternative) ? string.Empty : g.Alternative.Trim(),
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
                    Name = string.IsNullOrEmpty(g.Name) ? string.Empty : g.Name.Trim(),
                    Alternative = string.IsNullOrEmpty(g.Alternative) ? string.Empty : g.Alternative.Trim(),
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
                    Name = string.IsNullOrEmpty(resultQuery.Name) ? string.Empty : resultQuery.Name.Trim(),
                    Alternative = string.IsNullOrEmpty(resultQuery.Alternative) ? string.Empty : resultQuery.Alternative.Trim(),
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



        /// <summary> Fetch games by id </summary>
        /// <param name="request">request for fetch</param>
        [HttpGet]
        [Route("GetEmulById")]
        [ProducesResponseType(typeof(GameGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmulById([FromQuery] GameGetByIdRequest request)
        {
            var resultQuery = await Mediator.Send(new GetGameByIdQuery() { Id = request.Id });

            var name = resultQuery.Name;
            var platform = resultQuery.Platform.SmallName;
            var alias = platform.Replace("gbx", "gb").Replace("gen", "segaMD").Replace("sms", "segaMS");
            var links = resultQuery.GameLinks.Where(x => x.Type == Domain.Enums.TypeUrl.Rom);

            var dirExract = await ExtractRom(resultQuery);

            var r = new System.Text.RegularExpressions.Regex(@"(\([\w\s]*hack[\w\s]*\))",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

            var r2 = new System.Text.RegularExpressions.Regex(@"(\([\w\s]*prototype[\w\s]*\))",
               System.Text.RegularExpressions.RegexOptions.IgnoreCase |
               System.Text.RegularExpressions.RegexOptions.Multiline);

            var files = Directory.GetFiles(dirExract).Where(g => !r.IsMatch(g) && !r2.IsMatch(g)).ToArray();

            string mainrom = files.First();
            var mrom = files.Where(x => x.Contains("[!]") || x.Contains("(!)")).FirstOrDefault();
            if (!string.IsNullOrEmpty(mrom))
                mainrom = mrom;

            var rom = new Uri(new Uri(_baseUrl), $"CACHE/{platform}/{GetNameDir(name)}/{Path.GetFileName(mainrom)}").ToString();
           // var rom = GetLinks(resultQuery).Where(x => x.Type == "rom").FirstOrDefault().Url;

string html = @"
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <meta name=""robots"" content=""noindex"" />
    <base href=""/"" />
    <title>ColdCast Games</title>
</head>
<body>

<div style=""width:640px;height:480px;max-width:100%;margin:0 auto;"">
<div id=""game""></div>
</div>
"+Environment.NewLine+
@$"<script type=""text/javascript"">
    EJS_player = '#game';
    EJS_gameName = '{name.Replace("'", string.Empty)}';
    EJS_biosUrl = '';
    EJS_gameUrl = '{rom}';
    EJS_core = '{alias}';
    EJS_pathtodata = '/data/';
</script>

"+@"<script type=""text/javascript"" src=""data/loader.js""></script>
<style>
html,body {margin:0;padding:0;}
</style>
</body>
</html>";

            var resultCommand = await Mediator.Send(new CreateDownloadCommand() { Game = resultQuery });

            return Content(html, "text/html");
        }

        private async Task<string> ExtractRom(Domain.Entities.Game game)
        {
            var nameDir = GetNameDir(game.Name.Trim());
            var romfile = game.GameLinks.Where(x => x.Type == Domain.Enums.TypeUrl.Rom).FirstOrDefault().Url;

            var zipFile = Path.Combine(_filesPath, "ROMS", game.Platform.SmallName, romfile);
            var extractDir = Path.Combine(_filesPath, "CACHE", game.Platform.SmallName, nameDir);

            if (Directory.Exists(extractDir))
                if (Directory.GetFiles(extractDir).Any())
                    return await Task.FromResult(extractDir);

            return await Task.Run(() =>
            {
                Directory.CreateDirectory(extractDir);

                using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(extractDir, GetValidName(entry.FullName)), true);
                    }
                }

             //   ZipFile.ExtractToDirectory(zipFile, extractDir, true);
                return extractDir;
            });
        }

        private ICollection<GameLink> GetLinksWithCover(Domain.Entities.Game game)
        {
            var nameDir = GetNameDir(game.Name.Trim());
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
            var nameDir = GetNameDir(game.Name.Trim());
           
            return gameLinks?.Select(s => new GameLink()
            {
                Type = s.Type switch
                {
                    Domain.Enums.TypeUrl.Rom => "rom",
                    Domain.Enums.TypeUrl.Screen => "screen",
                    Domain.Enums.TypeUrl.Cover => "cover",
                    Domain.Enums.TypeUrl.CoverBack => "cover",
                    _ => throw new NotImplementedException(),
                },
                Url = s.Type switch
                {
                    Domain.Enums.TypeUrl.Rom => new Uri(new Uri(_baseUrl), $"ROMS/{game.Platform.SmallName}/{s.Url}").ToString(),
                    _ => new Uri(new Uri(_baseUrl), $"IMAGES/{game.Platform.SmallName}/{nameDir}/{s.Url}").ToString(),
                }
            }).ToList();
        }

        private string GetNameDir(string name) => name.Trim().Replace("'", "").Replace(':', ' ').Replace('\\', '_').Replace('/', '_')
            .Replace('?', ' ').Replace('<', '_').Replace('>', '_').Replace(' ', '_');

        private string GetValidName(string name) => name.Trim().Replace("'", "").Replace(':', '_').Replace('\\', '_').Replace('/', '_')
            .Replace('?', '_').Replace('<', '_').Replace('>', '_').Replace(' ', '_').Replace('"', '.');

    }
}
