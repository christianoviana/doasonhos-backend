using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Group;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.Models.Donor;
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
    public class GroupApplication
    {
        private IRepositoryAsync<Group> Repository { get; set; }
        private IMapper Mapper { get; set; }

        public GroupApplication(IRepositoryAsync<Group> repository,
                              IMapper mapper)
        {           
            this.Repository = repository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<GroupResponseDto>> GetAllGroups(FilterParams filterParams, PaginationParams paginationParams)
        {
            IQueryable<Group> groups = Repository.GetAllAsQueryable().OrderBy(g => g.Name);

            if (filterParams != null)
            {
                if (!string.IsNullOrEmpty(filterParams.Term))
                {
                    groups = groups.Where(g => g.Name.Contains(filterParams.Term, StringComparison.InvariantCultureIgnoreCase) ||
                                               g.Description.Contains(filterParams.Term, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            PagedResponse<GroupResponseDto> pagedResponse = new PagedResponse<GroupResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(groups, paginationParams, this.Mapper.Map<IEnumerable<GroupResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<GroupResponseDto>> GetGroupIn(List<Guid> groupIds)
        {
            var groups = await this.Repository.GetWhereAsync((item) => groupIds.Contains(item.Id));
            var groupsDto = this.Mapper.Map<IEnumerable<GroupResponseDto>>(groups);

            return groupsDto;
        }

        public async Task<GroupResponseDto> GetGroup(Expression<Func<Group, bool>> predicate)
        {
            List<Group> lstGroup = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstGroup = await query.ToListAsync();
            var group = lstGroup.FirstOrDefault();

            if (group == null) return null;

            var groupDto = this.Mapper.Map<GroupResponseDto>(group);

            return groupDto;
        }

        public async Task<IEnumerable<GroupItemsResponseDto>> GetGroupItems()
        {
            List<Group> lstGroup = null;
            var query = Repository.GetAllAsQueryable().Include(p => p.Items).Select(g => new Group()
            {
                Name = g.Name,
                Description = g.Description,
                Items = g.Items.Where(i => i.IsActive == true).ToList()
            }).OrderBy(g => g.Name);

            lstGroup = await query.ToListAsync();

            if (lstGroup == null) return null;

            var lstGroupItemsDto = this.Mapper.Map<IEnumerable<GroupItemsResponseDto>>(lstGroup);

            return lstGroupItemsDto;
        }

        public async Task<GroupItemsResponseDto> GetGroupItems(Expression<Func<Group, bool>> predicate)
        {
            List<Group> lstGroup = null;
            var query = Repository.GetWhereAsQueryable(predicate).Include(p => p.Items);
                      
            lstGroup = await query.ToListAsync();
            var group = lstGroup.FirstOrDefault();

            if (group == null) return null;

            var groupItemsDto = this.Mapper.Map<GroupItemsResponseDto>(group);

            return groupItemsDto;
        }

        public async Task<bool> ExistGroup(Expression<Func<Group, bool>> predicate)
        {
            List<Group> lstGroup = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstGroup = await query.ToListAsync();
            var group = lstGroup.FirstOrDefault();

            if (group == null) return false;            

            return true;
        }

        public async Task<Guid> CreateGroup(GroupRequestDto GroupDto)
        {
            var group = this.Mapper.Map<Group>(GroupDto);
            group.Id = Guid.NewGuid();

            await this.Repository.AddAsync(group);
            await this.Repository.SaveAsync();

            return group.Id;
        }

        public async Task UpdateGroup(Guid id, GroupRequestDto groupDto)
        {
            var group = this.Mapper.Map<Group>(groupDto);
            group.Id = id;

            this.Repository.Udate(group);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteGroup(GroupResponseDto groupDto)
        {
            var group = this.Mapper.Map<Group>(groupDto);

            this.Repository.Delete(group);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteGroupById(Guid id)
        {
            this.Repository.DeleteById(id);
            await this.Repository.SaveAsync();
        }
    }
}
