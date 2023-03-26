using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Core.Models;
using API.Core;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LuftbornDBContext _context;
        public CategoryRepository(LuftbornDBContext context)
        {
            _context = context;
        }

        public async Task Add(Category category)
        {
            var selectedTags = category.Tags.Select(fr => fr.Id);
            category.Tags = await _context.Tags
                    .Where(f => selectedTags.Contains(f.Id))
                    .ToListAsync();
            _context.Add(category);
        }
        public async Task Update(Category category)
        {
            var selectedTags = category.Tags.Select(fr => fr.Id);
            category.Tags = await _context.Tags
                    .Where(f => selectedTags.Contains(f.Id))
                    .ToListAsync();
        }
        public async Task<QueryResult<Category>> GetCategories(CategoryQuery categoryQuery)
        {
            var query = _context
                    .Categories
                    .Include(c => c.Tags)
                    .WhereIf(!string.IsNullOrEmpty(categoryQuery.Name), c => c.Name == categoryQuery.Name)
                    .AsQueryable();

            var totalItems = query.Count();

            var columnsMap = new Dictionary<string, Expression<Func<Category, object>>>()
            {
                ["name"] = c => c.Name,
            };

            query = query.ApplyOrdering(categoryQuery, columnsMap);
            query = query.ApplyPaging(categoryQuery);

            var result = new QueryResult<Category>();
            result.Items = query.ToList();
            result.TotalItems = totalItems;

            return result;
        }

        public async Task<Category> GetCategory(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await _context.Categories.FindAsync(id);

            return await _context
                            .Categories
                            .Include(p => p.Tags)
                            .SingleOrDefaultAsync(p => p.Id == id);
        }

        public void Remove(Category Category)
        {
            Category.IsDeleted = true;
        }
    }
}