using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RetroLauncher.WebAPI.Controllers.Games.Dto;

namespace RetroLauncher.WebAPI.Controllers.Games.Get
{
    /// <summary> Reponse with infromation about Genre games </summary>
    public class GamesGetResponse : PaginatedApiResponse
    {
        [JsonProperty("data", Required = Required.Always, Order = 4)]
        [Required]
        public GameData Data { get; set; } = new GameData();

        public GamesGetResponse()
        {
            Code = 200;
            Status = "OK";
        }
    }

    public class GameGetResponse : ApiResponse
    {
        [JsonProperty("data", Required = Required.Always, Order = 4)]
        [Required]
        public GameData Data { get; set; } = new GameData();

        public GameGetResponse()
        {
            Code = 200;
            Status = "OK";
        }
    }

    public class GameData
    {
        [JsonProperty("count", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Count { get; set; } = default!;

        [JsonProperty("games", Required = Required.Always)]
        [Required]
        public ICollection<Game> Games { get; set; } = new Collection<Game>();

    }
}
