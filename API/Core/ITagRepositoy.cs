using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Core.Models;

namespace API.Core
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTags();
    }
}