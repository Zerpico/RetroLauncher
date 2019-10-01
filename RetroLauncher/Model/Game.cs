using RetroLauncher.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Model
{
    public  class Game : IGame
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }        
        public int? Year { get; set; }
        public string Developer { get; set; }
        public string Annotation { get; set; }
        public int Downloads { get; set; }
        public double Rating { get; set; }

        public Genre Genre { get; set; }
        public Platform Platform { get; set; }
        public List<GameLink> GameLinks { get; set; }

        public string ImgUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url : string.Empty;
        public List<GameLink> Screens => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl != TypeUrl.Rom).ToList() : new List<GameLink>();

    }
}
