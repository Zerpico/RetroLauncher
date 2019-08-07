using System.Collections.Generic;

namespace RomsDownloaderGUI
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }
        string[] Prefix { get; set; }
    }

}
