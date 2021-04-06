using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Donation;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Donor;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PucMinas.Services.Charity.Extensions;

namespace PucMinas.Services.Charity.Application
{
    public class DonationApplication
    {
        private IRepositoryAsync<Donation> DonationRepository { get; set; }
        private IRepositoryAsync<DonationItem> DonationItemRepository { get; set; }
        private IRepositoryAsync<Item> ItemRepository { get; set; }

        private IMapper Mapper { get; set; }

        public DonationApplication(IRepositoryAsync<Donation> donationRepository,
                                  IRepositoryAsync<DonationItem> donationItemRepository,
                                  IRepositoryAsync<Item> itemRepository,
                                  IMapper mapper)
        {
            this.DonationRepository = donationRepository;
            this.DonationItemRepository = donationItemRepository;
            this.ItemRepository = itemRepository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<DonationResponseDto>> GetAllDonation(PaginationParams paginationParams)
        {
            IQueryable<Donation> donation = DonationRepository.GetAllAsQueryable().OrderBy(g => g.Date);
                       
            PagedResponse<DonationResponseDto> pagedResponse = new PagedResponse<DonationResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(donation, paginationParams, this.Mapper.Map<IEnumerable<DonationResponseDto>>);

            return pagedResponse;
        }

        public async Task<PagedResponse<DonationResponseDto>> GetAllDonation(Expression<Func<Donation, bool>> predicate, PaginationParams paginationParams)
        {
            IQueryable<Donation> donation = DonationRepository.GetWhereAsQueryable(predicate).Include(d => d.CharitableEntity).Include(d=> d.DonationItem).OrderByDescending(g => g.Date);

            PagedResponse<DonationResponseDto> pagedResponse = new PagedResponse<DonationResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(donation, paginationParams, this.Mapper.Map<IEnumerable<DonationResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<DonationResponseDto>> GetAllDonation(Expression<Func<Donation, bool>> predicate)
        {
            IQueryable<Donation> donation = DonationRepository.GetWhereAsQueryable(predicate).Include(d => d.CharitableEntity).Include(d => d.DonationItem).OrderByDescending(g => g.Date);

            IEnumerable <DonationResponseDto> donationResponse =  this.Mapper.Map<IEnumerable<DonationResponseDto>>(await donation.ToListAsync());

            return donationResponse;
        }

        public async Task<IEnumerable<DonationResponseDto>> GetDonationIn(List<Guid> DonationIds)
        {
            var donation = await this.DonationRepository.GetWhereAsync((item) => DonationIds.Contains(item.Id));
            var donationDto = this.Mapper.Map<IEnumerable<DonationResponseDto>>(donation);

            return donationDto;
        }

        public async Task<DonationResponseDto> GetDonation(Expression<Func<Donation, bool>> predicate)
        {
            List<Donation> lstDonation = null;
            var query = DonationRepository.GetWhereAsQueryable(predicate);
                      
            lstDonation = await query.ToListAsync();
            var donation = lstDonation.FirstOrDefault();

            if (donation == null) return null;

            var donationDto = this.Mapper.Map<DonationResponseDto>(donation);

            return donationDto;
        }

        public async Task<bool> ExistDonation(Expression<Func<Donation, bool>> predicate)
        {
            List<Donation> lstDonation = null;
            var query = DonationRepository.GetWhereAsQueryable(predicate);

            lstDonation = await query.ToListAsync();
            var donation = lstDonation.FirstOrDefault();

            if (donation == null) return false;

            return true;
        }

        public async Task<Guid> CreateDonation(DonationCreateDto donationDto, bool completed = false)
        {          
            var donation = this.Mapper.Map<Donation>(donationDto);
            donation.Id = Guid.NewGuid();
            donation.Date = DateTime.Now.ToBrazilianTimeZone();
            donation.Completed = completed;
            donation.Canceled = false;

            await this.DonationRepository.AddAsync(donation);

            if (donationDto.DonationItem != null && donationDto.DonationItem.Count() > 0)
            {
                foreach (var donateItem in donationDto.DonationItem)
                {
                    var item = await this.ItemRepository.GetByIdAsync(donateItem.ItemId);

                    if (item != null)
                    {
                        await this.DonationItemRepository.AddAsync(new DonationItem() { DonationId = donation.Id, ItemId = item.Id, Name = item.Name, Price = item.Price, Quantity = donateItem.ItemQuantity });                        
                    }
                }                                                    
            }          

            await this.DonationRepository.SaveAsync();

            return donation.Id;
        }

        public async Task UpdateDonation(Guid id, DonationUpdateDto donationDto)
        {
            var donation = this.Mapper.Map<Donation>(donationDto);
            donation.Id = id;

            var items = donationDto.DonationItem;

            if (items != null && items.Count() > 0)
            {
                // Removing all current items / donation
                var donationItems = await DonationItemRepository.GetWhereAsync(p => p.DonationId == donation.Id);

                if (donationItems != null)
                {
                    foreach (var item in donationItems)
                    {
                        DonationItemRepository.Delete(item);
                    }
                }
                // Adding all new items / donation
                foreach (var item in items)
                {
                    await this.DonationItemRepository.AddAsync(new DonationItem() { DonationId = donation.Id, ItemId = item.ItemId, Quantity = item.ItemQuantity });
                }
            }          

            this.DonationRepository.Udate(donation);
            await this.DonationRepository.SaveAsync();
        }

        public async Task CancelDonation(Guid id)
        {         
            var donationAsync = await DonationRepository.GetWhereAsync(p => p.Id == id);
            var donation = donationAsync.FirstOrDefault();

            if (donation == null)
            {
                throw new Exception($"Donation id ${id} was not found");
            }


            if (donation.Total > 0)
            {
                throw new Exception($"Online donation cannot be canceled by this function");
            }

            donation.Canceled = true;
            donation.Completed = false;

            this.DonationRepository.Udate(donation);
            await this.DonationRepository.SaveAsync();
        }

        public async Task ApproveDonation(Guid id)
        {
            var donationAsync = await DonationRepository.GetWhereAsync(p => p.Id == id);
            var donation = donationAsync.FirstOrDefault();

            if (donation == null)
            {
                throw new Exception($"Donation id ${id} was not found");
            }

            if (donation.Total > 0)
            {
                throw new Exception($"Online donation cannot be canceled by this function");
            }

            donation.Canceled = false;
            donation.Completed = true;

            this.DonationRepository.Udate(donation);
            await this.DonationRepository.SaveAsync();
        }


        public async Task DeleteCharity(Guid id)
        {
            this.DonationRepository.DeleteById(id);
            await this.DonationRepository.SaveAsync();
        }
    }
}
