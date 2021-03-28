using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Donor;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Extensions;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Application
{
    public class ItemApplication
    {
        private IRepositoryAsync<Item> Repository { get; set; }
        private IRepositoryAsync<CharitableInformationItem> CharitableItemRepository { get; set; }
        private IMapper Mapper { get; set; }

        public ItemApplication(IRepositoryAsync<Item> repository,
                               IRepositoryAsync<CharitableInformationItem> charitableItemRepository,
                              IMapper mapper)
        {           
            this.Repository = repository;
            this.CharitableItemRepository = charitableItemRepository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<ItemResponseDto>> GetAllItems(FilterParams filterParams, PaginationParams paginationParams, bool withGroups = true)
        {
            IQueryable<Item> items = null;
            items = Repository.GetAllAsQueryable();

            if (filterParams!= null)
            {
                if (!string.IsNullOrEmpty(filterParams.Term))
                {
                    items = items.Where(i => i.Name.Contains(filterParams.Term, StringComparison.InvariantCultureIgnoreCase) ||
                                             i.Description.Contains(filterParams.Term, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            if (withGroups)
            {
                items = items.Include(i => i.Group).OrderBy(i => i.Name);
            }
            else
            {
                items = items.OrderBy(i => i.Name);
            }
                       

            PagedResponse<ItemResponseDto> pagedResponse = new PagedResponse<ItemResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(items, paginationParams, this.Mapper.Map<IEnumerable<ItemResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<ItemResponseDto>> GetItemIn(List<Guid> itemIds)
        {
            var items = await this.Repository.GetWhereAsync((item) => itemIds.Contains(item.Id));
            var itemsDto = this.Mapper.Map<IEnumerable<ItemResponseDto>>(items);

            return itemsDto;
        }

        public async Task<IEnumerable<ItemResponseDto>> GetCharityItemIn(Guid charityInfoId, List<Guid> itemIds)
        {
            var items = this.CharitableItemRepository.GetWhereAsQueryable((item) => item.CharitableInformationId.Equals(charityInfoId)).Include(ci => ci.Item);
            var filterItems =  await items.Where((item) => itemIds.Contains(item.ItemId)).ToListAsync();

            var itemsDto = this.Mapper.Map<IEnumerable<ItemResponseDto>>(filterItems.Select(i=>i.Item));

            return itemsDto;
        }

        public async Task<ItemResponseDto> GetItem(Expression<Func<Item, bool>> predicate, bool withGroups = true)
        {
            List<Item> lstItem = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            if (withGroups)
                query = query.Include(e => e.Group);

            lstItem = await query.ToListAsync();
            var item = lstItem.FirstOrDefault();

            if (item == null) return null;

            var itemDto = this.Mapper.Map<ItemResponseDto>(item);

            return itemDto;
        }

        public async Task<bool> ExistItem(Expression<Func<Item, bool>> predicate)
        {
            List<Item> lstItem = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstItem = await query.ToListAsync();
            var item = lstItem.FirstOrDefault();

            if (item == null) return false;            

            return true;
        }

        public async Task<Guid> CreateItem(ItemCreateDto itemDto, HttpRequest request)
        {          
            var item = this.Mapper.Map<Item>(itemDto);
            
            item.Id = Guid.NewGuid();
            item.IsActive = true;
            item.ImagePath = string.Empty;

            if (itemDto.Photo != null && itemDto.Photo.Length > 0)
            {
                var folderName = Path.Combine("resources", "items");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fileName = string.Format($"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}_{itemDto.Photo.FileName.Trim()}");
                var imagePath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    item.ImagePath = imagePath;
                    item.ImageUrl = string.Format($"{GetResourcesItemUri(request)}/{fileName}");

                    await itemDto.Photo.CopyToAsync(stream);                    
                }
            }          

            await this.Repository.AddAsync(item);
            await this.Repository.SaveAsync();

            return item.Id;
        }

        private string GetResourcesItemUri(HttpRequest request)
        {
            UriBuilder builder = new UriBuilder(request.Scheme, request.Host.Host)
            {
                Path = "resources/items"
            };

            if (request.Host.Port.HasValue)
            {
                builder.Port = request.Host.Port.Value;
            }            

            return builder.Uri.ToString();
        }

        public async Task UpdateItem(Guid id, ItemUpdateDto itemDto)
        {
            var item = this.Mapper.Map<Item>(itemDto);
            var itemModel = this.Repository.GetAllAsQueryable().Where(e => e.Id.Equals(id)).First();

            item.Id = itemModel.Id;
            item.ImagePath = itemModel.ImagePath;
            item.ImageUrl = itemModel.ImageUrl;

            this.Repository.Udate(item);
            await this.Repository.SaveAsync();
        }
        
        public async Task UpdateItemImage(Guid id, IFormFile photo, HttpRequest request)
        {
            var itemModel = this.Repository.GetAllAsQueryable().Where(e => e.Id.Equals(id)).First();
            var oldPath = itemModel.ImagePath;
           
            if (photo != null && photo.Length > 0)
            {
                var folderName = Path.Combine("resources", "items");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fileName = string.Format($"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}_{photo.FileName.Trim()}");
                var imagePath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {

                    itemModel.ImagePath = imagePath;
                    itemModel.ImageUrl = string.Format($"{GetResourcesItemUri(request)}/{fileName}");

                    await photo.CopyToAsync(stream);
                }
            }
            else
            {
                itemModel.ImagePath = string.Empty;
                itemModel.ImageUrl = string.Empty;
            }

            if (!string.IsNullOrEmpty(oldPath))
            {
                var fileInfo = new FileInfo(oldPath);

                if (fileInfo.Exists)
                    fileInfo.Delete();
            }

            this.Repository.Udate(itemModel);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteItem(ItemResponseDto itemDto)
        {
            var item = this.Mapper.Map<Item>(itemDto);
            var itemModel = this.Repository.GetAllAsQueryable().Where(e => e.Id.Equals(itemDto.Id)).First();

            if (!string.IsNullOrEmpty(itemModel.ImagePath))
            {
                var fileInfo = new FileInfo(itemModel.ImagePath);

                if (fileInfo.Exists)
                    fileInfo.Delete();                
            }

            this.Repository.Delete(item);
            await this.Repository.SaveAsync();
        }

        public async Task DeleteItemById(Guid id)
        {
            var itemModel = this.Repository.GetAllAsQueryable().Where(e => e.Id.Equals(id)).First();

            if (!string.IsNullOrEmpty(itemModel.ImagePath))
            {
                var fileInfo = new FileInfo(itemModel.ImagePath);

                if (fileInfo.Exists)
                    fileInfo.Delete();
            }

            this.Repository.DeleteById(id);
            await this.Repository.SaveAsync();
        }
    }
}
