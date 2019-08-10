using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLauncher.Model
{
    public class GameLink
    {
        int linkId;
        public int LinkId
        {
            get { return linkId; }
            set { linkId = value; }
        }

        public string Url { get; set; }
        public TypeUrl TypeUrl { get; set; }
        public string UrlRemote { get; set; }

    }

    public enum TypeUrl
    {
        Rom = 1,
        MainScreen = 2,
        AlterScreen = 3
    }
}
