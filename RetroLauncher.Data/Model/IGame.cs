using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Data.Model
{
    public interface IGame
    {
        int GameId { get; set; }
        string Name { get; set; }
        string NameSecond { get; set; }
        string NameOther { get; set; }
        string Genre { get; set; }
        int? Year { get; set; }
        string Developer { get; set; }
        string Annotation { get; set; }
        int Downloads { get; set; }
        double Rating { get; set; }              

        Platform Platform { get; set; }
        List<GameLink> GameLinks { get; set; }
    }
}
