using RetroLauncher.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public string Annotation { get; set; }
        [NotMapped]
        public int Downloads { get; set; }
        [NotMapped]
        public double Rating { get; set; }

        [NotMapped]
        public Genre Genre { get; set; }
        [NotMapped]
        public Platform Platform { get; set; }
        [NotMapped]
        public List<GameLink> GameLinks { get; set; }

        public GamePath LocalPath { get; set; }

        public bool IsInstall { get { return LocalPath != null && !string.IsNullOrEmpty(LocalPath.LocalPath); } }

        public string LocalPathRom { get { return LocalPath != null ? LocalPath.LocalPath : null; } }

        [NotMapped]
        public string ImgUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.MainScreen).FirstOrDefault().Url : string.Empty;

        [NotMapped]
        public string RomUrl => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl == TypeUrl.Rom).FirstOrDefault().Url : string.Empty;
        [NotMapped]
        public List<GameLink> Screens => (GameLinks != null && GameLinks.Count > 0) ? GameLinks.Where(i => i.TypeUrl != TypeUrl.Rom).ToList() : new List<GameLink>();

    }

    public class GamePath
    {
        public int GamePathId { get; set; }
        public int GameId { get; set; }
        public string LocalPath { get; set; }
    }
}
