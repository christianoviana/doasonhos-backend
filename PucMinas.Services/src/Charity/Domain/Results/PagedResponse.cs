using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Domain.Results
{
    public class PagedResponse<T> : Response<IEnumerable<T>> where T : class
    {
        [JsonProperty("pagination", Order = 2)]
        public Pagination Pagination { get; private set; }

        public PagedResponse()
        {
            this.Pagination = new Pagination();
        }

        public async Task<PagedResponse<T>> ToPagedResponse<TIn>(IQueryable<TIn> data, PaginationParams pagination, Func<object, IEnumerable<T>> convert, bool paging = true) where TIn : class
        {
            if (convert == null)
                throw new ArgumentException("Convert function cannot be null");

            if (data.Count() == 0)
            {
                this.Pagination = null;
                this.Data = null;             
            }
            else
            {
                FillPagination(pagination, data.Count());
                IEnumerable<TIn> _data;

                if (paging)
                {
                    IQueryable<TIn> pagedResponse = data.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size);
                    _data = await pagedResponse.ToListAsync();
                }
                else
                    _data = await data.ToListAsync();
                
                this.Data = convert(_data);
            }

            return this;
        }

        public async Task<PagedResponse<T>> ToPagedResponse(IQueryable<T> data, PaginationParams pagination, bool paging = true) 
        {
            int count = this.Data.Count();

            if (data.Count() == 0)
            {
                this.Pagination = null;
                this.Data = null;
            }
            else
            {
                FillPagination(pagination, count);

                if (paging)
                {
                    IQueryable<T> pagedResponse = data.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size);

                    this.Data = await pagedResponse.ToListAsync();
                }
                else
                {
                    this.Data = await data.ToListAsync();
                }
            }

            return this;
        }

        private void FillPagination(PaginationParams parameters, int totalCount)
        {          
            this.Pagination.CurrentPage = parameters.Page;
            this.Pagination.PageSize = parameters.Size;
            this.Pagination.TotalCount = totalCount;
            this.Pagination.TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.Size);

            if (this.Pagination.CurrentPage > this.Pagination.TotalPages)
            {
                throw new ArgumentException("The current page cannot be greater than total pages size");
            }
        }
    }
}
