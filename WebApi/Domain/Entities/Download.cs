using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Download : BaseEntity
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public DateTime Dt { get; set; }
    }
}
