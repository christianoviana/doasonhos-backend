using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Infrastructure.Repository.Interfaces
{
    public interface IRepositoryAsync<T>
    {
        Task AddAsync(T model);
        void Udate(T model);
        void DeleteById(object id);
        void Delete(T model);
        void DeleteRange(IEnumerable<T> model);
        IQueryable<T> GetAllAsQueryable();
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetWhereAsQueryable(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        // Better create a unit of work to commit an operations with many repositories
        Task SaveAsync();
    }
}
