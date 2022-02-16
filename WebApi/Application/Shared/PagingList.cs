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
        public int Current { get; set; }
        public int Max { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
