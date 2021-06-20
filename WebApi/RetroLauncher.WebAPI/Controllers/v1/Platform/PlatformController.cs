using Application.Features.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.v1.Platform
{
    [ApiVersion("1")]
    public class PlatformController : BaseApiController
    {
        private readonly ILogger<PlatformController> _logger;

        public PlatformController(ILogger<PlatformController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns></returns>
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
                return BadRequest(new ErrorGetResponse() { ErrorMessage = "Not found items" });
            }

            var result = resultQuery.Select(g => new Models.Platform()
            {
                Id = g.Id,
                PlatformName = g.PlatformName,
                Alias = g.Alias
            });

            return Ok(new PlatformsGetResponse() { Items = result });
        }
    }
}
