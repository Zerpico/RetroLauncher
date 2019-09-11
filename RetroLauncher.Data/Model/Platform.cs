using RetroLauncher.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.Data.Model
{
    public class Platform 
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string Alias { get; set; }
    }
}
