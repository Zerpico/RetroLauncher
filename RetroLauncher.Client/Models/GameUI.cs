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
            this.NameSecond = string.IsNullOrEmpty(game.NameSecond) ? game.NameOther : game.NameSecond ;
            this.Year = game.Year;
            this.Developer = game.Developer;
            this.Annotation = game.Annotation;
            this.Downloads = game.Downloads;
            this.Rating = game.Rating;
            this.Genre = game.Genre;
            this.Platform = game.Platform;
            this.GameLinks = game.GameLinks;
        }

        public string IconPlatform 
        {
         
            get
            {
                var dir = System.AppDomain.CurrentDomain.BaseDirectory;
                switch (Platform.Alias)
                {
                    case "nes":
                        return System.IO.Path.Combine(dir, "icons\\nintendo_nes.png");
                    case "sms":
                        return System.IO.Path.Combine(dir, "icons\\sega_master_system.png");
                    case "gen":
                        return System.IO.Path.Combine(dir, "icons\\sega_genesis.png");
                    case "snes":
                        return System.IO.Path.Combine(dir, "icons\\nintendo_supernes.png");
                    case "gbc":
                        return System.IO.Path.Combine(dir, "icons\\nintendo_game_boy_pocket.png");
                    default:
                        return System.IO.Path.Combine(dir, "icons\\nintendo_nes.png");
                }
            }
        }
    
                

        public string ImgUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url : string.Empty;

        public string RomUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.Rom).FirstOrDefault().Url : string.Empty;

        public List<GameLink> Screens => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl != TypeUrl.Rom).ToList() : new List<GameLink>();
    }
}

