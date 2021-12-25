using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace RetroLauncher.WebAPI.Controllers.Genres.Dto
{
    public class Genre
    {
        [JsonProperty("id", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Id { get; set; } = default!;

        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; } = default!;
    }
}
