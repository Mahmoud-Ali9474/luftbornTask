using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Core.Models;
using API.Core;

using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TagRepository : ITagRepository
    {
        private readonly LuftbornDBContext _context;
        public TagRepository(LuftbornDBContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Tag>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}