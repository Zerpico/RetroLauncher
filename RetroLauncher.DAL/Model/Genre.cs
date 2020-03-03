using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.DAL.Model
{
    public class Genre
    {
        public Genre()
        {

        }


        public Genre(int genreId, string genreName)
        {
            GenreId = genreId;
            GenreName = genreName;
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public virtual ICollection<Game> Games { get; set; }


    }
}
