using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.DonorPJ;
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
    public class DonorPJApplication
    {
        private IRepositoryAsync<DonorPJ> repository { get; set; }
        private IRepositoryAsync<User> userRepository { get; set; }
        private IRepositoryAsync<Role> roleRepository { get; set; }
        private IRepositoryAsync<UserRole> userRoleRepository { get; set; }

        private IMapper mapper { get; set; }

        public DonorPJApplication(IRepositoryAsync<DonorPJ> repository,
                                  IRepositoryAsync<User> userRepository,
                                  IRepositoryAsync<Role> roleRepository,
                                  IRepositoryAsync<UserRole> userRoleRepository,
                                  IMapper mapper)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.mapper = mapper;
        }

        public async Task<PagedResponse<DonorPJResponseDto>> GetAllDonorsPJ(PaginationParams paginationParams)
        {
            IQueryable<DonorPJ> donorsPf = repository.GetAllAsQueryable();

            PagedResponse<DonorPJResponseDto> pagedResponse = new PagedResponse<DonorPJResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(donorsPf, paginationParams, this.mapper.Map<IEnumerable<DonorPJResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<DonorPJResponseDto>> GetDonorIn(List<Guid> donorPJIds)
        {
            var donorsPJ = await this.repository.GetWhereAsync((item) => donorPJIds.Contains(item.Id));
            var donorsPJDto = this.mapper.Map<IEnumerable<DonorPJResponseDto>>(donorsPJ);

            return donorsPJDto;
        }

        public async Task<DonorPJResponseDto> GetDonorPJ(Expression<Func<DonorPJ, bool>> predicate)
        {
            List<DonorPJ> lstDonorPJ = null;
            var query = repository.GetWhereAsQueryable(predicate);

            lstDonorPJ = await query.ToListAsync();
            var donorPJ = lstDonorPJ.FirstOrDefault();

            if (donorPJ == null) return null;

            var donorPJDto = this.mapper.Map<DonorPJResponseDto>(donorPJ);

            return donorPJDto;
        }

        public async Task<bool> ExistDonorPJ(Expression<Func<DonorPJ, bool>> predicate)
        {
            List<DonorPJ> lstDonorPJ = null;
            var query = repository.GetWhereAsQueryable(predicate);

            lstDonorPJ = await query.ToListAsync();
            var donorPJ = lstDonorPJ.FirstOrDefault();

            if (donorPJ == null) return false;

            return true;
        }

        public async Task<Guid> CreateDonorPJ(DonorPJCreateDto donorPJDto)
        {
            User user = new User(Guid.NewGuid(), donorPJDto.Login.ToLower(), donorPJDto.Password.ToSHA512(),
                                LoginType.DONOR_PJ, Guid.NewGuid(), true);          

            var donorPJ = this.mapper.Map<DonorPJ>(donorPJDto);
            donorPJ.Id = Guid.NewGuid();
            donorPJ.UserId = user.Id;

            var lstRole = await roleRepository.GetWhereAsync(r => r.Name.ToLower().Equals("donor_pj"));

            if (!lstRole.Any())
            {
                throw new Exception("Cannot find role donor_pj");
            }

            await userRoleRepository.AddAsync(new UserRole() { User = user, RoleId = lstRole.First().Id });
            await this.repository.AddAsync(donorPJ);
            await this.repository.SaveAsync();

            return donorPJ.Id;
        }

        public async Task UpdateDonorPJ(DonorPJResponseDto donorPJResponseDto, DonorPJUpdateDto donorPJDto)
        {
            var donorPJ = this.mapper.Map<DonorPJ>(donorPJDto);
            donorPJ.Id = donorPJResponseDto.Id;
            donorPJ.UserId = donorPJResponseDto.UserId;

            this.repository.Udate(donorPJ);
            await this.repository.SaveAsync();
        }

        public async Task DeleteDonorPJ(DonorPJResponseDto donorPJDto)
        {
            var donorPJ = this.mapper.Map<DonorPJ>(donorPJDto);

            this.repository.Delete(donorPJ);
            await this.repository.SaveAsync();
        }

        public async Task DeleteGroupById(Guid id)
        {
            this.repository.DeleteById(id);
            await this.repository.SaveAsync();
        }
    }
}
