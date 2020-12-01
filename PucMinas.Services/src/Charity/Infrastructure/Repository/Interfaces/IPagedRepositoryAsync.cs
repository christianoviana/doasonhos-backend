using PucMinas.Services.Charity.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Infrastructure.Repository.Interfaces
{
    public interface IPagedRepositoryAsync<T> : IRepositoryAsync<T>
    {
        Task<IEnumerable<T>> GetAllPagedPredicateAsync(PaginationParams parameters, Expression<Func<T, bool>> predicate);
    }
}
