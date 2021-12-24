﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebAPI.Models
{
    public partial class Game 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alternative { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string Annotation { get; set; }

        public string Genre { get; set; }
        public Platform Platform { get; set; }       
     //   public int Downloads { get; set; }
        public double? Ratings { get; set; }

     //   public IEnumerable<GameLink> GameLinks { get; set; }
    }
}