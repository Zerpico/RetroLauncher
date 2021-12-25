using Application.Features.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetroLauncher.WebAPI.Controllers.Genres.Dto;
using RetroLauncher.WebAPI.Controllers.Genres.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers
{   
    public class GenreController : BaseApiController
    {
        private readonly ILogger<GenreController> _logger;

        public GenreController(ILogger<GenreController> logger)
        {
            _logger = logger;
        }

        /// <summary> Fetch genres list </summary>
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
                return BadRequest(new ErrorGetResponse() { Code = 400, Status = "Not found items" });
            }

            var result = resultQuery
                .Select(g => new Genre()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToDictionary(d=>d.Id.ToString(), t=>new Genre() { Id = t.Id, Name = t.Name });

            return Ok(new GengresGetResponse() { Data = new GenreData() { Genres = result, Count=result.Count } });
        }
    }
}

