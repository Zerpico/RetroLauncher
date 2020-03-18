using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.Common.Model
{
    public class Platform
    {
        public Platform() { }
        public Platform(int platformId, string platformName, string alias) 
        {
            PlatformId = platformId;
            PlatformName = platformName;
            Alias = alias;
        }

        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string Alias { get; set; }
    }
}
