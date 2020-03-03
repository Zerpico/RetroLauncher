using System;
using System.Collections.Generic;

namespace RetroLauncher.WebApi.Model
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int RatingValue { get; set; }
        public DateTime Dt { get; set; }

        public virtual Game Game { get; set; }
        public virtual User User { get; set; }

    }
}
