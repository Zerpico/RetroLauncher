using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Common.Model
{
    public class Game 
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }
        public int? Year { get; set; }
        public string Developer { get; set; }
        public string Annotation { get; set; }
        public int? Downloads { get; set; }
        public double? Rating { get; set; }

       /* public int GenreId { get; set; }
        public int PlatformId { get; set; }*/

        public Genre Genre { get; set; }
        public Platform Platform { get; set; }
        public List<GameLink> GameLinks { get; set; }
    }
}
