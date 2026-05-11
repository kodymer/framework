using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Ddd.Domain.Common.Pagination
{
    public class PagedResult<T>
    {
        public Pagination Pagination { get; }
        public List<T> Data { get; }

        public PagedResult(List<T> data, Pagination pagination)
        {
            Data = data;
            Pagination = pagination;
        }
    }
}
