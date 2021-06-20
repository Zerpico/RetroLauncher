using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared
{
    public class PagingList<T> where T : BaseEntity
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
