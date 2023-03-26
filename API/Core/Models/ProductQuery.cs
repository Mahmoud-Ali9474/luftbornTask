using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Core.Models
{
    public class ProductQuery : QueryObject
    {
        public string? Title { get; set; }
        public DateTime? AddedDate { get; set; }
        public string? TagName { get; set; }
    }
}