﻿using AutoMapper;
using PucMinas.Services.Charity.Domain.DTO.Approval;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using PucMinas.Services.Charity.Domain.DTO.Donation;
using PucMinas.Services.Charity.Domain.DTO.DonorPF;
using PucMinas.Services.Charity.Domain.DTO.DonorPJ;
using PucMinas.Services.Charity.Domain.DTO.Group;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.DTO.Role;
using PucMinas.Services.Charity.Domain.DTO.User;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Approvals;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Donor;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.ValueObject;
using System;
using System.Globalization;
using System.Linq;

namespace PucMinas.Services.Charity.Domain.Mappers
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<UserCreateDto, User>();
            CreateMap<RoleDto, UserRole>().ForMember(d => d.Role, o => o.MapFrom(s => s));            

            CreateMap<RoleDto, Role>();
            CreateMap<Role, RoleDto>();

            CreateMap<Approval, ApprovalResponseDto>().ForMember(d => d.Status, o => o.MapFrom(s => (ApproverStatus) s.Status));

            CreateMap<Group, GroupResponseDto>();
            CreateMap<Group, GroupItemsResponseDto>().ForMember(d => d.Items, o => o.MapFrom(s => s.Items.Select(i => new ItemDto() { Id = i.Id, Name = i.Name, Description = i.Description, Price = i.Price, ImageURL = i.ImageUrl })));
            CreateMap<GroupResponseDto, Group>();
            CreateMap<GroupRequestDto, Group>();           

            CreateMap<ItemCreateDto, Item>().ForMember(d=> d.ImagePath, o => o.Ignore())
                                            .ForMember(d=> d.Price, o => o.MapFrom(i => Double.Parse(i.Price, CultureInfo.InvariantCulture)));
            CreateMap<ItemUpdateDto, Item>();

            CreateMap<Item, ItemResponseDto>();
            CreateMap<ItemResponseDto, Item>();

            CreateMap<DonorPFCreateDto, DonorPF>().ForMember(d => d.Address, o => o.MapFrom(s => new Address() { Country = s.Country, State = s.State, City = s.City }));
            CreateMap<DonorPFUpdateDto, DonorPF>().ForMember(d => d.Address, o => o.MapFrom(s => new Address() { Country = s.Country, State = s.State, City = s.City }));
                        
            CreateMap<DonorPF, DonorPFResponseDto>()
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Address.Country))
                .ForMember(d => d.State, o => o.MapFrom(s => s.Address.State))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<DonorPFResponseDto, DonorPF>();

            CreateMap<DonorPJCreateDto, DonorPJ>().ForMember(d => d.Address, o => o.MapFrom(s => new Address() { Country = s.Country, State = s.State, City = s.City }));
            CreateMap<DonorPJUpdateDto, DonorPJ>().ForMember(d => d.Address, o => o.MapFrom(s => new Address() { Country = s.Country, State = s.State, City = s.City }));

            CreateMap<DonorPJ, DonorPJResponseDto>()
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Address.Country))
                .ForMember(d => d.State, o => o.MapFrom(s => s.Address.State))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<DonorPJResponseDto, DonorPJ>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<CharitableEntity, CharityResponseDto>()
                 .ForMember(d => d.Active, o => o.MapFrom(s => s.IsActive))
                 .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                 .ForMember(d => d.CellPhone, o => o.MapFrom(s => s.ContactNumber.CellPhone))
                 .ForMember(d => d.Telephone, o => o.MapFrom(s => s.ContactNumber.Telephone))
                 .ForMember(d => d.Information, o => o.MapFrom(s => s.CharitableInformation));

            CreateMap<CharitableEntity, CharityStatusResponseDto>()
                 .ForMember(d => d.Active, o => o.MapFrom(s => s.IsActive))
                 .ForMember(d => d.HasCharityInformation, o => o.MapFrom(s => s.CharitableInformation != null ? true :false));

            CreateMap<CharityCreateDto, CharitableEntity>()
                .ForMember(d=> d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d=> d.ContactNumber, o => o.MapFrom(s => new ContactNumber(s.CellPhone, s.Telephone)));

            CreateMap<CharityUpdateDto, CharitableEntity>()
              .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
              .ForMember(d => d.ContactNumber, o => o.MapFrom(s => new ContactNumber(s.CellPhone, s.Telephone)));

            CreateMap<CharitableInformation, CharityInfoResponseDto>()
             .ForMember(d => d.Photo01, o => o.MapFrom(s => s.Photo01.ImageUrl))
             .ForMember(d => d.Photo02, o => o.MapFrom(s => s.Photo02.ImageUrl))
             .ForMember(d => d.TitlePhoto01, o => o.MapFrom(s => s.Photo01.Title))
             .ForMember(d => d.TitlePhoto02, o => o.MapFrom(s => s.Photo02.Title));

            CreateMap<CharityInfoUpdateDto, CharitableInformation>()
                .ForMember(d => d.Photo01, o => o.Ignore())
                .ForMember(d => d.Photo02, o => o.Ignore());

            CreateMap<CharityInfoCreateDto, CharitableInformation>()
               .ForMember(d => d.Photo01, o => o.Ignore())
               .ForMember(d => d.Photo02, o => o.Ignore());            

            CreateMap<User, UserResponseDto>().ForMember(d => d.Roles, o => o.MapFrom(s => s.UserRoles.Select(ur => ur.Role)));

            CreateMap<Donation, DonationCreateDto>();
            CreateMap<Donation, DonationResponseDto>()
                .ForMember(d => d.DonationItem, o => o.MapFrom(s => s.DonationItem.Select(di => new DonationItemResponseDto() { Name = di.Name, Price = di.Price, Quantity = di.Quantity })))
                .ForMember(d => d.CharitableEntity, o => o.MapFrom(s => s.CharitableEntity));

            CreateMap<DonationCreateDto, Donation>().ForMember(d => d.DonationItem, o => o.Ignore());
        }
    }
}
