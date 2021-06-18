using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Game : BaseEntity
    {      
        public string Name { get; set; }
        public string NameSecond { get; set; }
        public string NameOther { get; set; }      
        public int? Year { get; set; }
        public string Developer { get; set; }
        public string Annotation { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual ICollection<GameLink> GameLinks { get; set; }
        public virtual ICollection<Download> Downloads { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

    }
}
