using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.Games.Dto
{
    public class Game
    {
        [JsonProperty("id", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty("alternative")]
        [Required(AllowEmptyStrings = true)]
        public string Alternative { get; set; }

        [JsonProperty("year")]
        [Required(AllowEmptyStrings = true)]
        public string Year { get; set; }

        [JsonProperty("publisher")]
        [Required(AllowEmptyStrings = true)]
        public string Publisher { get; set; }

        [JsonProperty("annotation")]
        [Required(AllowEmptyStrings = true)]
        public string Annotation { get; set; }

        [JsonProperty("genres")]
        [Required(AllowEmptyStrings = true)]
        public ICollection<int> Genres { get; set; }

        [JsonProperty("platform", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Platform { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Include)]
        [Range(0, 5)]        
        public double? Ratings { get; set; }

        [JsonProperty("links")]
        [Required(AllowEmptyStrings = true)]
        public ICollection<GameLink> Links { get; set; }

    }
}
