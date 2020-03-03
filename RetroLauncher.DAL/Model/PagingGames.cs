using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.DAL.Model
{
    public class PagingGames
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public List<Game> Items { get; set; }
    }
}
