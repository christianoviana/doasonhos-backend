using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.DonorPF;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Donor;
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
    public class DonorPFApplication
    {
        private IRepositoryAsync<DonorPF> Repository { get; set; }
        private IRepositoryAsync<User> UserRepository { get; set; }
        private IRepositoryAsync<Role> RoleRepository { get; set; }
        private IRepositoryAsync<UserRole> UserRoleRepository { get; set; }

        private IMapper Mapper { get; set; }

        public DonorPFApplication(IRepositoryAsync<DonorPF> repository,
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

        public async Task<PagedResponse<DonorPFResponseDto>> GetAllDonorsPF(PaginationParams paginationParams)
        {
            IQueryable<DonorPF> donorsPf = Repository.GetAllAsQueryable().OrderBy(g => g.Name);

            PagedResponse<DonorPFResponseDto> pagedResponse = new PagedResponse<DonorPFResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(donorsPf, paginationParams, this.Mapper.Map<IEnumerable<DonorPFResponseDto>>);

            return pagedResponse;
        }

        public async Task<PagedResponse<DonorPFResponseDto>> GetAllDonorsPF(Expression<Func<DonorPF, bool>> predicate, PaginationParams paginationParams)
        {
            IQueryable<DonorPF> donorsPf = Repository.GetWhereAsQueryable(predicate).OrderBy(d => d.Name);

            PagedResponse<DonorPFResponseDto> pagedResponse = new PagedResponse<DonorPFResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(donorsPf, paginationParams, this.Mapper.Map<IEnumerable<DonorPFResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<DonorPFResponseDto>> GetAllDonorsPF(Expression<Func<DonorPF, bool>> predicate)
        {
            IQueryable<DonorPF> donorsPf = Repository.GetWhereAsQueryable(predicate).OrderBy(d => d.Name);

            IEnumerable<DonorPFResponseDto> lstDonorsPf = this.Mapper.Map<IEnumerable<DonorPFResponseDto>>(await donorsPf.ToListAsync());

            return lstDonorsPf;
        }      

        public async Task<IEnumerable<DonorPFResponseDto>> GetDonorIn(List<Guid> donorPFIds)
        {
            var donorsPF = await this.Repository.GetWhereAsync((item) => donorPFIds.Contains(item.Id));
            var donorsPFDto = this.Mapper.Map<IEnumerable<DonorPFResponseDto>>(donorsPF);

            return donorsPFDto;
        }

        public async Task<DonorPFResponseDto> GetDonorPF(Expression<Func<DonorPF, bool>> predicate)
        {
            List<DonorPF> lstDonorPF = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstDonorPF = await query.ToListAsync();
            var donorPF = lstDonorPF.FirstOrDefault();

            if (donorPF == null) return null;
            
            var donorPFDto = this.Mapper.Map<DonorPFResponseDto>(donorPF);

            return donorPFDto;
        }

        public async Task<bool> ExistDonorPF(Expression<Func<DonorPF, bool>> predicate)
        {
            List<DonorPF> lstDonorPF = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstDonorPF = await query.ToListAsync();
            var donorPF = lstDonorPF.FirstOrDefault();

            if (donorPF == null) return false;

            return true;
        }

        public async Task<Guid> CreateDonorPF(DonorPFCreateDto donorPFDto)
        {          
            User user = new User(Guid.NewGuid(), donorPFDto.Login.ToLower(), donorPFDto.Password.ToSHA512(), 
                                 LoginType.DONOR_PF, Guid.NewGuid(), true);                     

            var donorPF = this.Mapper.Map<DonorPF>(donorPFDto);
            donorPF.Id = Guid.NewGuid();
            donorPF.UserId = user.Id;

            var lstRole = await RoleRepository.GetWhereAsync(r => r.Name.ToLower().Equals("donor_pf"));

            if (!lstRole.Any())
            {
                throw new Exception("Cannot find role donor_pf");
            }

            await UserRoleRepository.AddAsync(new UserRole() { User = user, RoleId = lstRole.First().Id });           
            await this.Repository.AddAsync(donorPF);
            await this.Repository.SaveAsync();

            return donorPF.Id;
        }

        public async Task UpdateDonorPF(DonorPFResponseDto donorPFResponseDto, DonorPFUpdateDto donorPFDto)
        {
            var donorPF = this.Mapper.Map<DonorPF>(donorPFDto);
            donorPF.Id = donorPFResponseDto.Id;
            donorPF.UserId = donorPFResponseDto.UserId;

            this.Repository.Udate(donorPF);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteDonorPF(DonorPFResponseDto donorPFDto)
        {
            var donorPF = this.Mapper.Map<DonorPF>(donorPFDto);

            this.Repository.Delete(donorPF);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteGroupById(Guid id)
        {
            this.Repository.DeleteById(id);
            await this.Repository.SaveAsync();
        }
    }
}
