using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Role;
using PucMinas.Services.Charity.Domain.DTO.User;
using PucMinas.Services.Charity.Domain.Enums;
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
    public class UserApplication
    {
        private IRepositoryAsync<User> Repository { get; set; }
        private IRepositoryAsync<UserRole> UserRoleRepository { get; set; }
        private IRepositoryAsync<Role> RoleRepository { get; set; }
        private IMapper Mapper { get; set; }
        public const string DEFAULT_PASSWORD = "*****";

        public UserApplication(IRepositoryAsync<User> repository,
                              IRepositoryAsync<Role> roleRepository,
                              IRepositoryAsync<UserRole> userRoleRepository,
                              IMapper mapper)
        {
            this.Repository = repository;
            this.UserRoleRepository = userRoleRepository;
            this.RoleRepository = roleRepository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<UserResponseDto>> GetAllUsers(PaginationParams paginationParams, bool withRoles = true)
        {
            IQueryable<User> users = null;

            if (withRoles)            
                users = Repository.GetAllAsQueryable().OrderBy(u => u.Login).Include(e => e.UserRoles).ThenInclude(ur => ur.Role);
            else
                users = Repository.GetAllAsQueryable();

            PagedResponse<UserResponseDto> pagedResponse = new PagedResponse<UserResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(users, paginationParams, this.Mapper.Map<IEnumerable<UserResponseDto>>);

            return pagedResponse;
        }

        public async Task<UserResponseDto> GetUserDto(Expression<Func<User, bool>> predicate, bool withRoles = true)
        {
            List<User> lstUsers = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            if (withRoles)
                query = query.Include(e => e.UserRoles).ThenInclude(ur => ur.Role);

            lstUsers = await query.ToListAsync();
            var user = lstUsers.FirstOrDefault();

            if (user == null) return null;

            var userDto = this.Mapper.Map<UserResponseDto>(user);

            return userDto;
        }

        public async Task<UserOwnerDto> GetUserOwnerDto(Guid id)
        {
            List<User> lstUsers = null;
            var query = Repository.GetWhereAsQueryable(u => u.Id.Equals(id))
                                  .Include(p => p.DonorPF)
                                  .Include(p => p.DonorPJ)
                                  .Include(p => p.CharitableEntity);
           

            lstUsers = await query.ToListAsync();
            var user = lstUsers.FirstOrDefault();

            if (user == null) return null;

            var owner = new UserOwnerDto();
            owner.type = user.Type.ToString();

            switch (user.Type)
            {
                case LoginType.DONOR_PF:
                    owner.Id = user.DonorPF.Id;
                    owner.Name = user.DonorPF.Name;
                    break;
                case LoginType.DONOR_PJ:
                    owner.Id = user.DonorPJ.Id;
                    owner.Name = user.DonorPJ.CompanyName;
                    break;
                case LoginType.CHARITABLE_ENTITY:
                    owner.Id = user.CharitableEntity.Id;
                    owner.Name = user.CharitableEntity.Name;
                    break;
                case LoginType.MANAGER:
                case LoginType.ADMINISTRATOR:
                case LoginType.NONE:
                    owner.Id = Guid.Empty;
                    owner.Name = string.Empty;
                    break;
            }         

            return owner;
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> predicate, bool withRoles = true)
        {
            List<User> lstUsers = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            if (withRoles)
                query = query.Include(e => e.UserRoles).ThenInclude(ur => ur.Role);

            query = query.Include(p => p.DonorPF).Include(p => p.DonorPJ).Include(p => p.CharitableEntity);

            lstUsers = await query.ToListAsync();
            var user = lstUsers.FirstOrDefault();

            if (user != null)
                user.Password = DEFAULT_PASSWORD;

            return user;
        }

        public async Task UpdateUser(User user)
        {
            user.Login = user.Login.ToLower();

            this.Repository.Udate(user);
            await this.Repository.SaveAsync();           
        }

        public async Task DeleteUser(User user)
        {
            this.Repository.Delete(user);
            await this.Repository.SaveAsync();
        }      

        public async Task UpdateAllUserRoles(User user, IEnumerable<RoleDto> roles)
        {
            // Removing all current roles
            var userRoles = await UserRoleRepository.GetWhereAsync(ur => ur.UserId == user.Id);

            if (userRoles != null)
            {
                foreach (var uRole in userRoles)
                {
                    this.UserRoleRepository.Delete(uRole);
                }
            }
                                 
            // Apply new roles
            if (roles != null && roles.Count() > 0)
            {
                foreach (var role in roles)
                {
                    await this.UserRoleRepository.AddAsync(new UserRole() { UserId = user.Id, RoleId = role.Id });
                }
            }

            await this.UserRoleRepository.SaveAsync();
        }

        public async Task<User> CreateExternalUser(string login)
        {
            var user = new User();

            user.Id = Guid.NewGuid();
            user.Login = login.ToLower();
            user.ActivationCode = Guid.NewGuid();
            user.Password = "EXTERNAL_LOGIN";
            user.Type = LoginType.EXTERNAL;
            user.IsActive = true;
           
            await this.Repository.AddAsync(user);          
            await this.Repository.SaveAsync();

            return user;
        }

        public async Task<Guid> CreateUser(UserCreateDto userDto)
        {
            var user = this.Mapper.Map<User>(userDto);

            user.Id = Guid.NewGuid();
            user.Login = user.Login.ToLower();
            user.ActivationCode = Guid.NewGuid();
            user.Password = user.Password.ToSHA512();
            user.Type = (LoginType) Enum.Parse(typeof(LoginType), userDto.type);
            user.IsActive = true;

            if (userDto.Roles?.Count > 0)                
            {
                var lstRoles = await this.RoleRepository.GetWhereAsync((item) => userDto.Roles.Contains(item.Id));

                if (lstRoles != null && lstRoles.Count() > 0)
                {
                    foreach (var _role in lstRoles)
                    {
                        await this.UserRoleRepository.AddAsync(new UserRole() { User = user, RoleId = _role.Id });
                    }
                }
                else
                {
                    await this.Repository.AddAsync(user);
                }
            }         

            await this.Repository.SaveAsync();

            return user.Id;
        }
    }
}
