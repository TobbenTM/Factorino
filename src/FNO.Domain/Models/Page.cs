using System.Collections.Generic;

namespace FNO.Domain.Models
{
    public class Page<T>
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ResultCount { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
