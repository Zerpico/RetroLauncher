using RetroLauncher.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebApi.Model
{
    public class Game: IGame
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }
        public string Genre { get; set; }
        public int? Year { get; set; }
        public string Developer { get; set; }
        public string Annotation { get; set; }
        public int Downloads { get; set; }
        public double Rating { get; set; }

        public Platform Platform { get; set; }
        public List<GameLink> GameLinks { get; set; }
    }
}
