using System.Collections.Generic;
using Core.Common.GenericSearch;

namespace Core.Common.Models.Grid
{
    public sealed class GridSearch
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        
        public IEnumerable<Filter> Filters { get; set; }

        public Sort Sort { get; set; }
        
        public GridSearch(int pageIndex, int pageSize, IEnumerable<Filter> filters, Sort sort)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Filters = filters;
            Sort = sort;
        }

        public GridSearch()
        {
        }
    }
}