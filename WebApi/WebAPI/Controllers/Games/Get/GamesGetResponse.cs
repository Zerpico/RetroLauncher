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
/*    public class GamesGetResponse
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public IEnumerable<Game> Items { get; set; }
    }

    public class GameGetResponse
    {
        public Game Game { get; set; }
    }
*/

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

    public class GameData
    {
        [JsonProperty("count", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int Count { get; set; } = default!;

        [JsonProperty("games", Required = Required.Always)]
        [Required]
        public ICollection<Game> Games { get; set; } = new Collection<Game>();

    }

    /*
    public class GamesByGameID : PaginatedApiResponse
    {
        [JsonProperty("data", Required = Required.Always)]
        [Required]
        public Data Data { get; set; } = new Data();


        [JsonProperty("include", NullValueHandling = NullValueHandling.Include)]
        [Required]
        public Include Include { get; set; } = new Include();
      
    }
*/


    /*
        public partial class Include
        {
            [Newtonsoft.Json.JsonProperty("platform", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required]
            public Platform Platform { get; set; } = new Platform();

            [Newtonsoft.Json.JsonProperty("platform", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required]
            public ICollection<Genre> Genres { get; set; } = new Collection<Genre>();
        }
    */
}
