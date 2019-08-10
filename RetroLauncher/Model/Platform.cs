using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Model
{
    public class Platform
    {
        int platformId;
        public int PlatformId
        {
            get { return platformId; }
            set { platformId = value; }
        }

        public string PlatformName { get; set; }
        public string Alias { get; set; }
    }
}
