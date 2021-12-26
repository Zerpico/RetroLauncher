using Newtonsoft.Json;
using RetroLauncher.WebAPI.Controllers.Platforms.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RetroLauncher.WebAPI.Controllers.Platforms
{
    /// <summary> Reponse with infromation about Platform data </summary>
    public class PlatformsGetResponse : BaseApiResponse
    {
        [JsonProperty("data", Required = Required.Always, Order = 4)]
        [Required]
        public PlatformData Data { get; set; } = new PlatformData();

        public PlatformsGetResponse()
        {
            Code = 200;
            Status = "OK";
        }

    }

    public partial class PlatformData
    {
        [JsonProperty("count", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Count { get; set; } = default!;

        [JsonProperty("platforms", Required = Required.Always)]
        [Required]
        public IDictionary<string, Platform> Platforms { get; set; } = new Dictionary<string, Platform>();
    }
}
