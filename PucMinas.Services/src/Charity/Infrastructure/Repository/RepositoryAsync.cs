using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Infrastructure.Entity;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Infrastructure.Repository
{
    public class RepositoryAsync<T> : IDisposable, IRepositoryAsync<T> where T : class
    {
        protected ApplicationDbContext _context;

        public RepositoryAsync(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(T model)
        {
            if (model == null)
            {
                throw new ArgumentException("The model cannot be null");
            }

            await this._context.Set<T>().AddAsync(model);
        }

        public void DeleteById(object id)
        {
            T model = this._context.Set<T>().Find(id);

            if (model == null)
            {
                throw new ArgumentException($"The entity id={id} cannot be found");
            }
            else
            {
                this._context.Set<T>().Remove(model);
            }
        }

        public void Delete(T model)
        {
            if (model == null)
            {
                throw new ArgumentException($"The model cannot be null");
            }                      

            this._context.Set<T>().Attach(model);
            this._context.Entry(model).State = EntityState.Deleted;
        }

        public void Udate(T model)
        {
            //this._context.Set<T>().Attach(model);
            //this._context.Entry(model).State = EntityState.Modified;

            this._context.Set<T>().Update(model);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._context.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return this._context.Set<T>().AsNoTracking();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await this._context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetWhereAsQueryable(Expression<Func<T, bool>> predicate)
        {
            return this._context.Set<T>().AsNoTracking().Where(predicate);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await this._context.Set<T>().FindAsync(id);
        }

        // Better create a unit of work to commit an operations with many repositories
        

        public async Task SaveAsync()
        {
           await this._context.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }        
    }
}
