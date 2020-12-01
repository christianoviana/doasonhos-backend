using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Infrastructure.Entity;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Infrastructure.Repository
{
    public class PagedRepositoryAsync<T> : RepositoryAsync<T>, IPagedRepositoryAsync<T> where T : class
    {
        public PagedRepositoryAsync(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<T>> GetAllPagedPredicateAsync(PaginationParams parameters, Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> data = await this.GetWhereAsync(predicate);
            return data.Skip((parameters.Page - 1) * parameters.Size).Take(parameters.Size);
        }
        
    }
}
