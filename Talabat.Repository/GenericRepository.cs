using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesContract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext context;

        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>) await context.Set<Product>()
                    .Include(p => p.Brand)
                    .Include(p => p.Category).ToListAsync();
            }
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.getQuery(context.Set<T>(), spec).ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await context.Set<Product>()
                    .Where(p => p.Id == id)
                    .Include(p => p.Brand)
                    .Include(p => p.Category).FirstOrDefaultAsync() as T;
            }

            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.getQuery(context.Set<T>(), spec).FirstOrDefaultAsync();
        }
    }
}
