using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class GenreLink : BaseEntity
    {        
        public int GameId { get; set; }
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Game Game { get; set; }
    }
}
