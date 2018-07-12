using ConsoleManager.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.API.Queries
{
    public class QueryContactsResponse
    {
        public IEnumerable<Contact> Items { get; set; } = Enumerable.Empty<Contact>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }

        public int PageCount => (int)Math.Ceiling(TotalCount / (decimal)PageSize);
    }
}
