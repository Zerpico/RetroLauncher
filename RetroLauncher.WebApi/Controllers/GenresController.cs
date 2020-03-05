using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroLauncher.WebApi.Model;
using RetroLauncher.WebApi.Service;

namespace RetroLauncher.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private IDataRepository repository;

        public GenresController(DbLibraryGamesContext context)
        {
            repository = new SQLRepository(context);
        }
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {     
            return Ok(repository.GetGenres());
        }
    }
}