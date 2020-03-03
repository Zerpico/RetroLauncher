using System;
using System.Collections.Generic;

namespace RetroLauncher.WebApi.Model
{
    public partial class GameLink
    {
        public int LinkId { get; set; }
        public int GameId { get; set; }
        public string Url { get; set; }
        public int TypeUrl { get; set; }

        public virtual Game Game { get; set; }
    }
}
