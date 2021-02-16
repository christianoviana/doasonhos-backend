using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Approval;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Approvals;
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
        private IRepositoryAsync<Approval> ApprovalRepository { get; set; }        
        private IRepositoryAsync<UserRole> UserRoleRepository { get; set; }

        private IMapper Mapper { get; set; }

        public CharitableEntityApplication(IRepositoryAsync<CharitableEntity> repository,
                                  IRepositoryAsync<User> userRepository,
                                  IRepositoryAsync<Role> roleRepository,
                                  IRepositoryAsync<Approval> approvalRepository,
                                  IRepositoryAsync<UserRole> userRoleRepository,
                                  IMapper mapper)
        {
            this.Repository = repository;
            this.UserRepository = userRepository;
            this.RoleRepository = roleRepository;
            this.ApprovalRepository = approvalRepository;
            this.UserRoleRepository = userRoleRepository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<CharityResponseDto>> GetAllCharities(Expression<Func<CharitableEntity, bool>> predicate, FilterParams filterParams, PaginationParams paginationParams, bool withInformation = false)
        {
            IQueryable<CharitableEntity> charitableEntities = null;

            if (filterParams == null || string.IsNullOrEmpty(filterParams.Term))
            {
                charitableEntities = Repository.GetWhereAsQueryable(predicate).OrderBy(g => g.Name);
            }
            else
            {
                charitableEntities = Repository.GetWhereAsQueryable(predicate)
                                                .Where(c => c.Name.ToLower().Contains(filterParams.Term.ToLower()) || c.Cnpj.ToLower().Contains(filterParams.Term.ToLower()) || c.CharitableInformation.Nickname.ToLower().Contains(filterParams.Term.ToLower()))
                                                .OrderBy(g => g.Name);
            }

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

        public async Task<IEnumerable<ApprovalResponseDto>> GetCharityApprovalResponseDto(Expression<Func<Approval, bool>> predicate)
        {
            List<Approval> lstApprovals = null;
            var query = ApprovalRepository.GetWhereAsQueryable(predicate).OrderByDescending(a => a.Date);       

            lstApprovals = await query.ToListAsync();

            if (lstApprovals == null) return null;

            var approvalsDto = this.Mapper.Map<IEnumerable<ApprovalResponseDto>>(lstApprovals);

            return approvalsDto;
        }

        public async Task<IEnumerable<Approval>> GetCharityApprovals(Expression<Func<Approval, bool>> predicate)
        {
            IEnumerable<Approval> lstApproval = null;
            var query = ApprovalRepository.GetWhereAsQueryable(predicate);

            lstApproval = await query.ToListAsync();

            if (lstApproval == null || lstApproval.Count() == 0) return null;
            
            return lstApproval;
        }

        public async Task<CharityStatusResponseDto> GetCharityStatus(Expression<Func<CharitableEntity, bool>> predicate)
        {
            List<CharitableEntity> lstCharities = null;
            var query = Repository.GetWhereAsQueryable(predicate).Include(c => c.CharitableInformation);
                     
            lstCharities = await query.ToListAsync();
            var charity = lstCharities.FirstOrDefault();

            if (charity == null) return null;

            var charityDto = this.Mapper.Map<CharityStatusResponseDto>(charity);

            return charityDto;
        }

        public async Task<IEnumerable<PendingCharityByStatesDto>> GetCharityPendingByState()
        {
            var query = Repository.GetAllAsQueryable().Include(p => p.Approvals).Where(c => c.Status == ApproverStatus.PENDING || c.Status == ApproverStatus.ANALYZING);

            var charityByState = await query.GroupBy(g => g.Address.State).ToListAsync();

            if (charityByState == null)
            {
                return null;
            }

            IEnumerable<PendingCharityByStatesDto> pendings = charityByState.OrderBy(g => g.Key)
                                                                            .Select(g => new PendingCharityByStatesDto() { State = g.Key, charities = g.OrderBy(c => c.Approvals.OrderBy(a => a.Date).First().Date).Select(c => this.Mapper.Map<CharityResponseDto>(c)) });

            return pendings;
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

            var approval = new Approval()
            {
                Id = Guid.NewGuid(),
                CharitableEntityId = charitable.Id,
                Date = DateTime.Now,
                Message = "Análise Pendente",
                Status = (int)ApproverStatus.PENDING           
            };

            var lstRole = await RoleRepository.GetWhereAsync(r => r.Name.ToLower().Equals("charitable_entity"));

            if (!lstRole.Any())
            {
                throw new Exception("Cannot find role charitable_entity");
            }

            await UserRoleRepository.AddAsync(new UserRole() { User = user, RoleId = lstRole.First().Id });
            await this.Repository.AddAsync(charitable);
            await ApprovalRepository.AddAsync(approval);

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

        public async Task UpdateCharityStatus(Guid id, CharityStatusRequestDto charityStatusRequestDto)
        {
            var charityModel = Repository.GetWhereAsQueryable(c => c.Id.Equals(id)).First();
            charityModel.IsActive = charityStatusRequestDto.Active;

            this.Repository.Udate(charityModel);
            await this.Repository.SaveAsync();
        }

        public async Task UpdateCharityPending(Guid id, CharityApproveDto charityApproveDto)
        {
            var charityModel = Repository.GetWhereAsQueryable(c => c.Id.Equals(id)).First();
            var status = (ApproverStatus) Enum.Parse(typeof(ApproverStatus), charityApproveDto.Status);

            var approval = new Approval()
            {
                Id = Guid.NewGuid(),
                CharitableEntityId = charityModel.Id,
                Date = DateTime.Now,
                Message = charityApproveDto.Message,
                Detail = charityApproveDto.Detail,
                Status = (int)status
            };

            await ApprovalRepository.AddAsync(approval);

            charityModel.ApproverData = DateTime.Now;
            charityModel.Approver = charityApproveDto.ApproverName;
            charityModel.Status = status;

            switch (status)
            {
                case ApproverStatus.APPROVED:
                    charityModel.IsActive = true;
                    break;
                default:
                    charityModel.IsActive = false;
                    break;
            }
            this.Repository.Udate(charityModel);

            await this.Repository.SaveAsync();
        }

        public async Task DeleteCharity(Guid id)
        {
            this.Repository.DeleteById(id);
            await this.Repository.SaveAsync();
        }
        
    }
}
