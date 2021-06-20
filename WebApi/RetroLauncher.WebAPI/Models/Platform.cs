using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string PlatformName { get; set; }
        public string Alias { get; set; }
        public string UrlPic { get; set; }

    }
}
