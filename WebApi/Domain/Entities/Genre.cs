using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Genre : BaseEntity
    {
        public string GenreName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
