using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.v1.Game
{
    public class GamesGetRequest
    {
        public string Name { get; set; } = string.Empty;
        public int[] Genres { get; set; } = null;
        public int[] Platforms { get; set; } = null;

        [Range(1,100)]
        public int Limit { get; set; } = 50;

      //  [RegularExpression(@"^[1-9]$")]
        public int Offset { get; set; } = 0;
    }

    public class GameGetRequest
    {
        public int Id { get; set; }
    }
}
