﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Controllers.Genre.Get
{
    public class GengresGetResponse
    {
        public IEnumerable<Models.Genre> Items { get; set; }
    }
}