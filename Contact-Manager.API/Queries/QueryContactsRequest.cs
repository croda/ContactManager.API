using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.API.Queries
{
    public class QueryContactsRequest
    {
        private const int DEFAULT_PAGE_SIZE = 5;
        private const int MAX_PAGE_SIZE = 100;

        private string _searchString = "";
        public string SearchString
        {
            get => _searchString;
            set => _searchString = value ?? "";
        }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        private int _pageSize = DEFAULT_PAGE_SIZE;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > MAX_PAGE_SIZE)
                    _pageSize = MAX_PAGE_SIZE;
                else if (value < 1)
                    _pageSize = DEFAULT_PAGE_SIZE;
                else
                    _pageSize = value;
            }
        }

        internal int SkipCount => (Page - 1) * PageSize;
        internal int TakeCount => PageSize;
    }
}
