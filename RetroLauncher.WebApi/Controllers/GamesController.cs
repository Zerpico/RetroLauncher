using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RetroLauncher.WebApi.Model;
using RetroLauncher.WebApi.Service;

namespace RetroLauncher.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : Controller
    {
        private IDataRepository repository;

        public GamesController(DbLibraryGamesContext context)
        {            
            repository = new SQLRepository(context);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name, [FromQuery] int[] genres , [FromQuery] int[] platforms, [Range(1, 100)]int limit = 50, int offset = 0)
        {
            return Ok(repository.GetGames(name, genres, platforms, limit, offset));
        }


        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(repository.GetGameById(id));
        }
    }
}