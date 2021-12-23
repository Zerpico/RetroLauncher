using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.Platform
{
    public class PlatformsGetResponse
    {
        public IEnumerable<Models.Platform> Items { get; set; }
    }
}
