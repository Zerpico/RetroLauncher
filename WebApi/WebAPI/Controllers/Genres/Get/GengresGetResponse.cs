using Newtonsoft.Json;
using RetroLauncher.WebAPI.Controllers.Genres.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetroLauncher.WebAPI.Controllers.Genres.Get
{
    /// <summary> Reponse with infromation about Genre data </summary>
    public partial class GengresGetResponse : BaseApiResponse
    {
        [JsonProperty("data", Required = Required.Always, Order = 4)]
        [Required]
        public GenreData Data { get; set; } = new GenreData();
       
        public GengresGetResponse()
        {
            Code = 200;
            Status = "OK";
        }

    }

    public partial class GenreData
    {
        [JsonProperty("count", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Count { get; set; } = default!;

        [JsonProperty("genres", Required = Required.Always)]
        [Required]
        public IDictionary<string, Genre> Genres { get; set; } = new Dictionary<string, Genre>();
    }
}
