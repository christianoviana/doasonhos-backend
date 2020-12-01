using AutoMapper;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using PucMinas.Services.Charity.Domain.DTO.DonorPF;
using PucMinas.Services.Charity.Domain.DTO.DonorPJ;
using PucMinas.Services.Charity.Domain.DTO.Group;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.DTO.Role;
using PucMinas.Services.Charity.Domain.DTO.User;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Donor;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.ValueObject;
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

            CreateMap<Group, GroupResponseDto>();
            CreateMap<Group, GroupItemsResponseDto>().ForMember(d => d.Items, o => o.MapFrom(s => s.Items.Select(i => new ItemDto() { Id = i.Id, Name = i.Name, Description = i.Description, Price = i.Price, ImageURL = i.ImageUrl })));
            CreateMap<GroupResponseDto, Group>();
            CreateMap<GroupRequestDto, Group>();           

            CreateMap<ItemCreateDto, Item>().ForMember(d=> d.ImagePath, o => o.Ignore());
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
                 .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                 .ForMember(d => d.CellPhone, o => o.MapFrom(s => s.ContactNumber.CellPhone))
                 .ForMember(d => d.Telephone, o => o.MapFrom(s => s.ContactNumber.Telephone))
                 .ForMember(d => d.Information, o => o.MapFrom(s => s.CharitableInformation));



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

            CreateMap<User, UserResponseDto>().ForMember(d => d.Roles, o => o.MapFrom(s => s.UserRoles.Select(ur => ur.Role)));
        }
    }
}
