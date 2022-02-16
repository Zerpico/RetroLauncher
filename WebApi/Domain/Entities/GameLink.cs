using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public partial class GameLink : BaseEntity
    {
        public string Url { get; set; }
        public TypeUrl Type { get; set; }
        public Game Game { get; set; }
    }
}
