using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class User : BaseEntity
    {
        public User()
        {
            Downloads = new HashSet<Download>();
            Ratings = new HashSet<Rating>();            
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid MachineSid { get; set; }

        public ICollection<Download> Downloads { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
