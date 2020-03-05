using System;
using System.Collections.Generic;

namespace RetroLauncher.WebApi.Model
{
    public partial class User
    {
        public User()
        {
            Downloads = new HashSet<Download>();
            Ratings = new HashSet<Rating>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid MachineSid { get; set; }

        public virtual ICollection<Download> Downloads { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
