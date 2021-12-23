using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class GameLink : BaseEntity
    {
        public string Url { get; set; }
        public TypeUrl Type { get; set; }

        public Game Game { get; set; }
    }
}
