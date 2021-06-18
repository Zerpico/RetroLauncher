using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Platform : BaseEntity
    {
        public string PlatformName { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
