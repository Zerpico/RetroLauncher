using System;
using System.Collections.Generic;

namespace RetroLauncher.WebApi.Model
{
    public partial class Game
    {
        public Game()
        {
            GameLinks = new HashSet<GameLink>();
            Downloads = new HashSet<Download>();
            Ratings = new HashSet<Rating>();
        }

        public int GameId { get; set; }
        public string GameName { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }
        public int PlatformId { get; set; }
        public int GenreId { get; set; }
        public int? Year { get; set; }
        public string Developer { get; set; }
        public string Annotation { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual ICollection<GameLink> GameLinks { get; set; }
        public virtual ICollection<Download> Downloads { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

    }
}
