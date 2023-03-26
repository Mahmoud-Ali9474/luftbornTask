using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Extensions;
using API.Core.Models;
using API.Core;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly LuftbornDBContext _context;
        public ProductRepository(LuftbornDBContext context)
        {
            _context = context;
        }

        public async Task Add(Product product)
        {

            var selectedTags = product.Tags.Select(fr => fr.Id);
            product.Tags = await _context.Tags
                .Where(f => selectedTags.Contains(f.Id))
                .ToListAsync();
            _context.Add(product);
        }
        public async Task Update(Product product)
        {
            var selectedTags = product.Tags.Select(fr => fr.Id);
            product.Tags = await _context.Tags
                    .Where(f => selectedTags.Contains(f.Id))
                    .ToListAsync();
        }
        public async Task<QueryResult<Product>> GetAllProducts(ProductQuery productQuery)
        {
            var query = _context
                    .Products
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .WhereIf(!string.IsNullOrEmpty(productQuery.TagName), p => p.Tags.Any(t => t.Name.Contains(productQuery.TagName!)))
                    .WhereIf(productQuery.AddedDate.HasValue
                        , p => productQuery.AddedDate.Value.Date == p.AddedDate.Date)
                    .WhereIf(!string.IsNullOrEmpty(productQuery.Title?.Trim()), p => p.Title.Contains(productQuery.Title!.Trim()))
                    .AsQueryable();

            var totalItems = query.Count();

            var columnsMap = new Dictionary<string, Expression<Func<Product, object>>>()
            {
                ["price"] = p => p.Price,
                ["category"] = p => p.Category.Name,
                ["addedDate"] = p => p.AddedDate,
                ["title"] = p => p.Title
            };

            query = query.ApplyOrdering(productQuery, columnsMap);
            query = query.ApplyPaging(productQuery);

            var result = new QueryResult<Product>();
            result.Items = query.ToList();
            result.TotalItems = totalItems;

            return result;
        }

        public async Task<Product> GetProduct(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await _context.Products.FindAsync(id);

            return await _context
                            .Products
                            .Include(p => p.Category)
                            .Include(p => p.Tags)
                            .SingleOrDefaultAsync(p => p.Id == id);
        }

        public void Remove(Product product)
        {
            product.IsDeleted = true;
        }

        public void DeleteMany(List<int> ids)
        {
            var entitiesToDelete = _context.Products.Where(p => ids.Contains(p.Id));
            foreach (var item in entitiesToDelete)
            {
                item.IsDeleted = true;
            }
        }
    }
}