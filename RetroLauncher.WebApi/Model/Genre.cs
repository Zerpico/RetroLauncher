using System;
using System.Collections.Generic;

namespace RetroLauncher.WebApi.Model
{
    public partial class Genre
    {
        public Genre()
        {
            Games = new HashSet<Game>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
