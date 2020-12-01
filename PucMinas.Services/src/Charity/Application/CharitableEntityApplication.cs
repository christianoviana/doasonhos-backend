using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Extensions;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Application
{
    public class CharitableEntityApplication
    {
        private IRepositoryAsync<CharitableEntity> Repository { get; set; }
        private IRepositoryAsync<User> UserRepository { get; set; }
        private IRepositoryAsync<Role> RoleRepository { get; set; }
        private IRepositoryAsync<UserRole> UserRoleRepository { get; set; }

        private IMapper Mapper { get; set; }

        public CharitableEntityApplication(IRepositoryAsync<CharitableEntity> repository,
                                  IRepositoryAsync<User> userRepository,
                                  IRepositoryAsync<Role> roleRepository,
                                  IRepositoryAsync<UserRole> userRoleRepository,
                                  IMapper mapper)
        {
            this.Repository = repository;
            this.UserRepository = userRepository;
            this.RoleRepository = roleRepository;
            this.UserRoleRepository = userRoleRepository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<CharityResponseDto>> GetAllCharities(PaginationParams paginationParams, bool withInformation = false)
        {
            IQueryable<CharitableEntity> charitableEntities = Repository.GetAllAsQueryable().OrderBy(g => g.Name);

            if (withInformation)
            {
                charitableEntities = charitableEntities.Include(p => p.CharitableInformation);
            }

            PagedResponse<CharityResponseDto> pagedResponse = new PagedResponse<CharityResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(charitableEntities, paginationParams, this.Mapper.Map<IEnumerable<CharityResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<CharityResponseDto>> GetCharityIn(List<Guid> CharityIds)
        {
            var charity = await this.Repository.GetWhereAsync((item) => CharityIds.Contains(item.Id));
            var charityDto = this.Mapper.Map<IEnumerable<CharityResponseDto>>(charity);

            return charityDto;
        }

        public async Task<CharityResponseDto> GetCharity(Expression<Func<CharitableEntity, bool>> predicate, bool withInformation = false)
        {
            List<CharitableEntity> lstCharities = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            if (withInformation)
            {
                query = query.Include(p => p.CharitableInformation);
            }         

            lstCharities = await query.ToListAsync();
            var charity = lstCharities.FirstOrDefault();

            if (charity == null) return null;

            var charityDto = this.Mapper.Map<CharityResponseDto>(charity);

            return charityDto;
        }

        public async Task<bool> ExistCharity(Expression<Func<CharitableEntity, bool>> predicate)
        {
            List<CharitableEntity> lstCharities = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstCharities = await query.ToListAsync();
            var charity = lstCharities.FirstOrDefault();

            if (charity == null) return false;

            return true;
        }

        public async Task<Guid> CreateCharity(CharityCreateDto charityDto)
        {
            User user = new User(Guid.NewGuid(), charityDto.Login.ToLower(), charityDto.Password.ToSHA512(),
                                 LoginType.CHARITABLE_ENTITY, Guid.NewGuid(), true);

            var charitable = this.Mapper.Map<CharitableEntity>(charityDto);
            charitable.Status = ApproverStatus.PENDING;
            charitable.IsActive = false;
            charitable.ApproverData = null;
            charitable.Approver = string.Empty;
            charitable.Id = Guid.NewGuid();
            charitable.UserId = user.Id;

            var lstRole = await RoleRepository.GetWhereAsync(r => r.Name.ToLower().Equals("charitable_entity"));

            if (!lstRole.Any())
            {
                throw new Exception("Cannot find role charitable_entity");
            }

            await UserRoleRepository.AddAsync(new UserRole() { User = user, RoleId = lstRole.First().Id });
            await this.Repository.AddAsync(charitable);
            await this.Repository.SaveAsync();

            return charitable.Id;
        }

        public async Task UpdateCharity(Guid id, CharityUpdateDto charityDto)
        {
            var charityModel = Repository.GetWhereAsQueryable(c => c.Id.Equals(id)).First();
            var charitableEntity = this.Mapper.Map<CharitableEntity>(charityDto);

            charitableEntity.Id = charityModel.Id;
            charitableEntity.UserId = charityModel.UserId;

            this.Repository.Udate(charitableEntity);
            await this.Repository.SaveAsync();
        }

            public async Task DeleteCharity(Guid id)
            {
                this.Repository.DeleteById(id);
                await this.Repository.SaveAsync();
            }
    }
}
