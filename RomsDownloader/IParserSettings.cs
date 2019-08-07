using System.Collections.Generic;

namespace RomsDownloader
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }
        string[] Prefix { get; set; }
    }

}
