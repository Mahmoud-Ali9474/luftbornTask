using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Core.Models
{
    public class CategoryQuery : QueryObject
    {
        public string? Name { get; set; }
    }
}