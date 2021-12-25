using Application.Features.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RetroLauncher.WebAPI.Controllers.Platforms;
using RetroLauncher.WebAPI.Controllers.Platforms.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers
{
    public class PlatformController : BaseApiController
    {
        private readonly ILogger<PlatformController> _logger;

        public PlatformController(ILogger<PlatformController> logger)
        {
            _logger = logger;
        }

        /// <summary> Fetch platforms list </summary>
        [HttpGet]
        [Route("getList")]
        [ProducesResponseType(typeof(PlatformsGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorGetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var resultQuery = await Mediator.Send(new GetAllPlatformQuery());
            if (resultQuery == null)
            {
                _logger.LogError("Not found items");
                return BadRequest(new ErrorGetResponse() { Code = 200, Status = "Not found items" });
            }

            var result = resultQuery
                .Select(g => new Platform()
                {
                    Id = g.Id,
                    Name = g.Name,
                    Alias = g.SmallName
                })
                .ToDictionary(k => k.Id.ToString(), t => new Platform() { Id = t.Id, Name = t.Name, Alias = t.Alias });

            return Ok(new PlatformsGetResponse() { Data = new PlatformData() { Count = result.Count, Platforms = result } });
        }
    }
}
