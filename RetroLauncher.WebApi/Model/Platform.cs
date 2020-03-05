using System;
using System.Collections.Generic;

namespace RetroLauncher.WebApi.Model
{
    public partial class Platform
    {
        public Platform()
        {
            Games = new HashSet<Game>();
        }

        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
