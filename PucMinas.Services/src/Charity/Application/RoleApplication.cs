using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Role;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Application
{
    public class RoleApplication
    {
        private IRepositoryAsync<Role> Repository { get; set; }
        private IMapper Mapper { get; set; }

        public RoleApplication(IRepositoryAsync<Role> repository,
                              IMapper mapper)
        {           
            this.Repository = repository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<RoleDto>> GetAllRoles(FilterParams filterParams, PaginationParams paginationParams)
        {
            IQueryable<Role> roles = Repository.GetAllAsQueryable().OrderBy(r => r.Name);

            if (filterParams != null)
            {
                if (!string.IsNullOrEmpty(filterParams.Term))
                {
                    roles = roles.Where(r => r.Name.Contains(filterParams.Term, StringComparison.InvariantCultureIgnoreCase) ||
                                             r.Description.Contains(filterParams.Term, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            PagedResponse<RoleDto> pagedResponse = new PagedResponse<RoleDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(roles, paginationParams, this.Mapper.Map<IEnumerable<RoleDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<RoleDto>> GetRoleIn(List<Guid> rolesIds)
        {
            var roles = await this.Repository.GetWhereAsync((item) => rolesIds.Contains(item.Id));
            var rolesDto = this.Mapper.Map<IEnumerable<RoleDto>>(roles);

            return rolesDto;
        }

        public async Task<RoleDto> GetRole(Expression<Func<Role, bool>> predicate)
        {
            List<Role> lstRoles = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstRoles = await query.ToListAsync();
            var role = lstRoles.FirstOrDefault();

            if (role == null) return null;

            var roleDto = this.Mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public async Task<bool> ExistRole(Expression<Func<Role, bool>> predicate)
        {
            List<Role> lstRoles = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstRoles = await query.ToListAsync();
            var role = lstRoles.FirstOrDefault();

            if (role == null) return false;
                      
            return true;
        }

        public async Task<Guid> CreateRole(RoleDto roleDto)
        {
            var role = this.Mapper.Map<Role>(roleDto);
            role.Id = Guid.NewGuid();

            await this.Repository.AddAsync(role);
            await this.Repository.SaveAsync();

            return role.Id;
        }

        public async Task UpdateRole(RoleDto roleDto)
        {
            var role = this.Mapper.Map<Role>(roleDto);

            this.Repository.Udate(role);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteRoleById(Guid id)
        {
            this.Repository.DeleteById(id);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteRole(RoleDto roleDto)
        {
            var role = this.Mapper.Map<Role>(roleDto);

            this.Repository.Delete(role);
            await this.Repository.SaveAsync();
        }
    }
}
