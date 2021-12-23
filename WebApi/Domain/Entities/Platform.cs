using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Platform : BaseEntity
    {
        public string Name { get; set; }
        public string SmallName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
