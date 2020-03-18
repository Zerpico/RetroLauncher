using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RetroLauncher.WebApi.Model;

namespace RetroLauncher.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private DbLibraryGamesContext repository;

        public GenresController(DbLibraryGamesContext repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public  IActionResult Get()
        {
            var query = from db in repository.Genres
                        select new RetroLauncher.Common.Model.Genre(db.GenreId, db.GenreName);


            return Ok(query);
        }
    }
}