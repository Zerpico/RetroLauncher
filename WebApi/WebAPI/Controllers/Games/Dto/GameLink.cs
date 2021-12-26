

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace RetroLauncher.WebAPI.Controllers.Games.Dto
{
    public class GameLink
    {
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; }
    }

}
