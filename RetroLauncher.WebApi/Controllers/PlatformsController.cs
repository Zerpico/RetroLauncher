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
    public class PlatformsController : Controller
    {
        private DbLibraryGamesContext repository;

        public PlatformsController(DbLibraryGamesContext repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var query = from db in repository.Platforms
                        select new RetroLauncher.DAL.Model.Platform(db.PlatformId, db.PlatformName, db.Alias);


            return Ok(query);
        }
    }
}