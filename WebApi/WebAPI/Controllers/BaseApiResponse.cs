using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers
{
    public abstract class BaseApiResponse
    {
        [JsonProperty("code", Required = Required.Always, Order = 1)]
        [Range(0, int.MaxValue)]
        public int Code { get; set; } = default!;

        [JsonProperty("status", Required = Required.Always, Order = 2)]
        [Required(AllowEmptyStrings = true)]
        public string Status { get; set; } = default!;
    }

   
    public partial class PaginatedApiResponse : BaseApiResponse
    {
        [JsonProperty("pages", Required = Required.Always, Order = 3)]
        [Required]
        public Pages Pages { get; set; } = new Pages();
    }

    public partial class Pages
    {       

        [JsonProperty("current", NullValueHandling = NullValueHandling.Include)]
        [Required(AllowEmptyStrings = true)]
        [Range(1, int.MaxValue)]
        public int Current { get; set; } = 1;

        [JsonProperty("max", NullValueHandling = NullValueHandling.Include)]
        [Required(AllowEmptyStrings = true)]
        [Range(1, int.MaxValue)]
        public int Max { get; set; } = 1;
    }

    public partial class ErrorGetResponse: BaseApiResponse
    {
        
    }

}
