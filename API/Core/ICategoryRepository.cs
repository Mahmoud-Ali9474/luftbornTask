using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Core.Models;
namespace API.Core
{
    public interface ICategoryRepository
    {
        Task<QueryResult<Category>> GetCategories(CategoryQuery categoryQuery);
        Task<Category> GetCategory(int id, bool includeRelated = true);
        Task Add(Category Category);
        Task Update(Category Category);
        void Remove(Category Category);
    }
}