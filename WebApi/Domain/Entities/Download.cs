using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Download : BaseEntity
    {   
        public DateTime Dt { get; set; }

        public Game Game { get; set; }
        public User User { get; set; }

    }
}
