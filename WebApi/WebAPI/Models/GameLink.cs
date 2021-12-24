using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Models
{
    public class GameLink
    {
        public string GameId { get; set; }
        public TypeUrl Type { get; set; }
        public string Url { get; set; }        
    }

    public enum TypeUrl : int
    {
        Rom = 1,
        Cover = 2,
        Screen = 3
    }
}
