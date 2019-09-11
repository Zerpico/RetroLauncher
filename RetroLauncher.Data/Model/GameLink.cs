using RetroLauncher.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.Data.Model
{
    public class GameLink 
    {
        public int LinkId { get; set; }
        public string Url { get; set; }
        public TypeUrl TypeUrl { get; set; }
    }

    public enum TypeUrl
    {
        Rom = 1,
        MainScreen = 2,
        AlterScreen = 3
    }
}
