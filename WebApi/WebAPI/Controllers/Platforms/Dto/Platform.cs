using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace RetroLauncher.WebAPI.Controllers.Platforms.Dto
{
    public class Platform
    {
        [JsonProperty("id", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Id { get; set; } = default!;

        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; } = default!;

        [JsonProperty("alias", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Alias { get; set; } = default!;
    }
}
