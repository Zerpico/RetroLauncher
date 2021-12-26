using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.Games.Get
{
    /// <summary> Request to game by id </summary>
    public class GameGetByIdRequest
    {
        /// <summary> id of list </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Include)]
        [Range(1, int.MaxValue)]
        public int Id { get; set; } = 1;
    }

    /// <summary> Request to all games with pages </summary>
    public class GamesGetRequest
    {
        /// <summary> Number of page </summary>
        [JsonProperty("Page", NullValueHandling = NullValueHandling.Include)]
        [Range(1, int.MaxValue)]        
        public int Page { get; set; } = 1;

        /// <summary> delimited fields: annotations, publisher </summary>
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Include)]
        public string Fields { get; set; } = string.Empty;
    }

    /// <summary> Request to games search by name with pages </summary>
    public class GamesGetByNameRequest : GamesGetRequest
    {
        /// <summary> Search term </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; set; } = string.Empty;

        /// <summary> Filter genres </summary>
        [JsonProperty("genres", NullValueHandling = NullValueHandling.Include)]
        public int[] Genres { get; set; } = null;

        /// <summary> Filter platforms </summary>
        [JsonProperty("platforms", NullValueHandling = NullValueHandling.Include)]
        public int[] Platforms { get; set; } = null;
    }
}
