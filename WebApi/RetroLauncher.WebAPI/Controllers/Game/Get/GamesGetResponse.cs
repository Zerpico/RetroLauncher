using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.Game.Get
{
    public class GamesGetResponse
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public IEnumerable<Models.Game> Items { get; set; }
    }

    public class GameGetResponse
    {
        public Models.Game Game { get; set; }
    }
}
