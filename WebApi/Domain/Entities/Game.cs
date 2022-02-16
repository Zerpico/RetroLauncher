using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Game : BaseEntity
    {                       
        public string Name { get; set; }
        public string Alternative { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string Players { get; set; }
        public double? Rate { get; set; }
        public string Annotation { get; set; }

        public virtual ICollection<GenreLink> GenreLinks { get; set; }
        public Platform Platform { get; set; }
        public virtual ICollection<GameLink> GameLinks { get; set; }
        public virtual ICollection<Download> Downloads { get; set; }
    }
}
