using Application.Features.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetroLauncher.WebAPI.Controllers.v1.Genre.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.v1.Genre
{
    [ApiVersion("1")]
    public class GenreController : BaseApiController
    {
        private readonly ILogger<GenreController> _logger;

        public GenreController(ILogger<GenreController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getList")]
        [ProducesResponseType(typeof(GengresGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var resultQuery = await Mediator.Send(new GetAllGenresQuery());
            if (resultQuery == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { ErrorMessage = "Not found items" });
            }

            var result = resultQuery.Select(g => new Models.Genre()
            {
                Id = g.Id,
                GenreName = g.GenreName
            });

            return Ok(new GengresGetResponse() { Items = result });
        }
    }
}

