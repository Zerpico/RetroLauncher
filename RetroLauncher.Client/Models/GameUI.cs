using System.Collections.Generic;
using System.Linq;
using RetroLauncher.DAL.Model;

namespace RetroLauncher.Client.Models
{
    public class GameUI : Game
    {
        public GameUI (Game game )
        {
            this.GameId = game.GameId;
            this.Name = game.Name;
            this.NameOther = game.NameOther;
            this.NameSecond = game.NameSecond;
            this.Year = game.Year;
            this.Developer = game.Developer;
            this.Annotation = game.Annotation;
            this.Downloads = game.Downloads;
            this.Rating = game.Rating;
            this.Genre = game.Genre;
            this.Platform = new PlatformUI(game.Platform);
            this.GameLinks = game.GameLinks;
        }

        public new PlatformUI Platform { get; set; }

        public string ImgUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url : string.Empty;

        public string RomUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.Rom).FirstOrDefault().Url : string.Empty;

        public List<GameLink> Screens => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl != TypeUrl.Rom).ToList() : new List<GameLink>();
    }
}

