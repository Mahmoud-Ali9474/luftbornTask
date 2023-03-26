using System.Collections.Generic;

namespace API.Dtos
{
    public class QueryResultDto<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
        public QueryResultDto()
        {
            Items = new List<T>();
        }
    }
}