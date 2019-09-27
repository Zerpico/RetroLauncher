using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetroLauncher.Data;
using RetroLauncher.Data.Model;
using RetroLauncher.Data.Service;
using RetroLauncher.WebApi.Service;

namespace RetroLauncher.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IRepository _gbLibrary = new DbBaseRepository();

        // GET: api/Games
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "error", "not yet available" };
        }

        // GET: api/Games/GetGenres
        [HttpGet("GetGenres")]
        public async Task<IEnumerable<string>> GetGenres()
        {
            return await _gbLibrary.GetGenres();
        }

        // GET: api/Games/GetPlatforms
        [HttpGet("GetPlatforms")]
        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            return await _gbLibrary.GetPlatforms();
        }

        // GET: api/Games/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IGame> Get(int id)
        {
            return await _gbLibrary.GetGameById(id);
        }

        // GET: api/Games/GetFilter?name=NAME&genre=GENRE&platform=id&count=100&skip=100
        [HttpGet("GetFilter")]
        public async Task<(int, IEnumerable<IGame>)> GetFilter([FromQuery]FilterGame filter)
        {
            return await _gbLibrary.GetBaseFilter(filter);
        }

        

        /*

        // POST: api/Games
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
