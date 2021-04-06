using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Domain.ValueObject;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Application
{
    public class CharitableInformationApplication
    {
        private IRepositoryAsync<CharitableInformation> Repository { get; set; }
        private IRepositoryAsync<CharitableInformationItem> CharitableItemRepository { get; set; }

        private IMapper Mapper { get; set; }

        public CharitableInformationApplication(IRepositoryAsync<CharitableInformation> repository,
                                  IRepositoryAsync<CharitableInformationItem> charitableItemRepository,
                                  IMapper mapper)
        {
            this.Repository = repository;
            this.CharitableItemRepository = charitableItemRepository;
            this.Mapper = mapper;
        }

        public async Task<PagedResponse<CharityInfoResponseDto>> GetAllCharitiesInfo(PaginationParams paginationParams)
        {
            IQueryable<CharitableInformation> charitableEntities = Repository.GetAllAsQueryable().OrderBy(g => g.Nickname);

            PagedResponse<CharityInfoResponseDto> pagedResponse = new PagedResponse<CharityInfoResponseDto>();
            pagedResponse = await pagedResponse.ToPagedResponse(charitableEntities, paginationParams, this.Mapper.Map<IEnumerable<CharityInfoResponseDto>>);

            return pagedResponse;
        }

        public async Task<IEnumerable<CharityInfoResponseDto>> GetCharityInfoIn(List<Guid> CharityInfoIds)
        {
            var charityInfo = await this.Repository.GetWhereAsync((ci) => CharityInfoIds.Contains(ci.Id));
            var charityInfoDto = this.Mapper.Map<IEnumerable<CharityInfoResponseDto>>(charityInfo);

            return charityInfoDto;
        }

        public async Task<CharityInfoResponseDto> GetCharityInfo(Expression<Func<CharitableInformation, bool>> predicate)
        {
            List<CharitableInformation> lstCharitiesInfo = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstCharitiesInfo = await query.ToListAsync();
            var charityInfo = lstCharitiesInfo.FirstOrDefault();

            if (charityInfo == null) return null;

            var charityInfoDto = this.Mapper.Map<CharityInfoResponseDto>(charityInfo);

            return charityInfoDto;
        }

        public async Task<bool> ExistCharityInfo(Expression<Func<CharitableInformation, bool>> predicate)
        {
            List<CharitableInformation> lstCharitiesInfo = null;
            var query = Repository.GetWhereAsQueryable(predicate);

            lstCharitiesInfo = await query.ToListAsync();
            var charity = lstCharitiesInfo.FirstOrDefault();

            if (charity == null) return false;

            return true;
        }

        public async Task<CharityInfoItemResponseDto> GetCharityInfoItem(Expression<Func<CharitableInformationItem, bool>> predicate)
        {
            List<CharitableInformationItem> lstCharitiesInfoItem = null;
            var query = CharitableItemRepository.GetWhereAsQueryable(predicate).Include(p => p.Item).ThenInclude(p => p.Group);

            lstCharitiesInfoItem = await query.ToListAsync();

            if (lstCharitiesInfoItem == null || lstCharitiesInfoItem.Count == 0)
            {
                return new CharityInfoItemResponseDto() { items = new List<ItemResponseDto>() };
            }

            var lstItems = lstCharitiesInfoItem.Select(e => e.Item).Where(i => i.IsActive.Equals(true)).ToList();

            var lstCharityInfoItemDto = Mapper.Map<IEnumerable<ItemResponseDto>>(lstItems);

            var lstItemsDto = new CharityInfoItemResponseDto();
            lstItemsDto.items = lstCharityInfoItemDto.ToList();

            return lstItemsDto;
        }

        public async Task UpdateCharityInfoItem(Guid charitableEntityId, CharityInfoItemDto charityInfoDto)
        {
            var charityInfo = Repository.GetWhereAsQueryable(c => c.CharitableEntityId == charitableEntityId).First();
                             
            if (charityInfoDto.items != null)
            {
                // Removing all current charities / items
                var charityItems = await CharitableItemRepository.GetWhereAsync(p => p.CharitableInformationId == charityInfo.Id);

                if (charityItems != null)
                {
                    CharitableItemRepository.DeleteRange(charityItems);
                }

                foreach (var item in charityInfoDto.items)
                {
                    await CharitableItemRepository.AddAsync(new CharitableInformationItem() { CharitableInformationId = charityInfo.Id, ItemId = item });
                }
            }
            
            await this.Repository.SaveAsync();
        }
        
        public async Task<Guid> UpdateCharityInfo(Guid charitableEntityId, CharityInfoUpdateDto charityInfoDto, HttpRequest request)
        {
            var charitableInfo = this.Mapper.Map<CharitableInformation>(charityInfoDto);
            var query = Repository.GetWhereAsQueryable(c => c.CharitableEntityId == charitableEntityId).First();

            charitableInfo.PicturePath = query.PicturePath;
            charitableInfo.PictureUrl = query.PictureUrl;
            charitableInfo.Photo01 = query.Photo01;
            charitableInfo.Photo02 = query.Photo02;

            if (charitableInfo.Photo01 != null)
                charitableInfo.Photo01.Title = charityInfoDto.TitlePhoto01;

            if (charitableInfo.Photo02 != null)
                charitableInfo.Photo02.Title = charityInfoDto.TitlePhoto02;
        
            charitableInfo.CharitableEntityId = charitableEntityId;
            charitableInfo.Id = query.Id;
                    
            this.Repository.Udate(charitableInfo);
            await this.Repository.SaveAsync();

            return charitableInfo.Id;
        }

        public async Task<Guid> UpdateCharityInfoImage(Guid charitableEntityId, CharityInfoImageUpdateDto charityInfoDto,
                                                       CharityInfoResponseDto charityInfoResponseDto, HttpRequest request)
        {
            var charitableInformation = await Repository.GetByIdAsync(charityInfoResponseDto.Id);
                                  
            var imageName = charityInfoDto.Name.ToLower();
            string oldImage = string.Empty;

            var shortId = charitableInformation.Id.ToString().Split('-')[0];
            Tuple<string, string> result = null;

            switch (imageName)
            {
                case "picture":
                    result = await UploadImage(charityInfoDto.Photo, request, string.Format($"{shortId}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_picture{new FileInfo(charityInfoDto.Photo.FileName).Extension}"));

                    oldImage = charitableInformation.PicturePath;

                    charitableInformation.PicturePath = result.Item1;
                    charitableInformation.PictureUrl = result.Item2;
                    break;
                case "image01":
                    result = await UploadImage(charityInfoDto.Photo, request, string.Format($"{shortId}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_photo1{new FileInfo(charityInfoDto.Photo.FileName).Extension}"));

                    oldImage = charitableInformation.Photo01?.ImagePath;

                    charitableInformation.Photo01 = new Image()
                    {
                        ImagePath = result.Item1,
                        ImageUrl = result.Item2,
                        Title = charitableInformation?.Photo01?.Title
                    };
                    break;
                case "image02":
                    result = await UploadImage(charityInfoDto.Photo, request, string.Format($"{shortId}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_photo2{new FileInfo(charityInfoDto.Photo.FileName).Extension}"));

                    oldImage = charitableInformation.Photo02?.ImagePath;

                    charitableInformation.Photo02 = new Image()
                    {
                        ImagePath = result.Item1,
                        ImageUrl = result.Item2,
                        Title = charitableInformation?.Photo02?.Title
                    };
                    break;
            }

            charitableInformation.CharitableEntityId = charitableEntityId;
            charitableInformation.Id = charityInfoResponseDto.Id;

            try
            {
                this.Repository.Udate(charitableInformation);
                await this.Repository.SaveAsync();
            }
            catch (Exception)
            {
                try
                {
                    deleteImage(imageName, charitableInformation);
                }
                catch (Exception) { }

                throw;
            }
            finally
            {
                try
                {
                    deleteImage(oldImage);
                }
                catch (Exception) { }
            }

            return charitableInformation.Id;
        }

        public async Task<Guid> CreateCharityInfo(Guid charitableEntityId, CharityInfoCreateDto charityInfoDto, HttpRequest request)
        {
            var charitableInfo = this.Mapper.Map<CharitableInformation>(charityInfoDto);
            charitableInfo.CharitableEntityId = charitableEntityId;
            charitableInfo.Id = Guid.NewGuid();
                                             
            charitableInfo.PicturePath = null;
            charitableInfo.PictureUrl = null;
            charitableInfo.Photo01 = null;
            charitableInfo.Photo02 = null;

            var shortId = charitableInfo.Id.ToString().Split('-')[0];

            var picture = await UploadImage(charityInfoDto.Picture, request, string.Format($"{shortId}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_picture{new FileInfo(charityInfoDto.Picture.FileName).Extension}"));
            var photo01 = await UploadImage(charityInfoDto.Photo01, request, string.Format($"{shortId}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_photo1{new FileInfo(charityInfoDto.Picture.FileName).Extension}"));
            var photo02 = await UploadImage(charityInfoDto.Photo02, request, string.Format($"{shortId}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_photo2{new FileInfo(charityInfoDto.Picture.FileName).Extension}"));

            charitableInfo.PicturePath = picture.Item1;
            charitableInfo.PictureUrl = picture.Item2;

            charitableInfo.Photo01 = new Image()
            {
                ImagePath = photo01.Item1,
                ImageUrl = photo01.Item2,
                Title = charityInfoDto.TitlePhoto01
            };

            charitableInfo.Photo02 = new Image()
            {
                ImagePath = photo02.Item1,
                ImageUrl = photo02.Item2,
                Title = charityInfoDto.TitlePhoto02
            };
            
            try
            {
                await this.Repository.AddAsync(charitableInfo);
                await this.Repository.SaveAsync();
            }
            catch (Exception)
            {
                try
                {
                    deleteAllImages(charitableInfo);
                }
                catch (Exception){}

                throw;
            }     

            return charitableInfo.Id;
        }
           
        public async Task DeleteCharityInfo(Guid id)
        {
            this.Repository.DeleteById(id);
            await this.Repository.SaveAsync();
        }

        private static void deleteAllImages(CharitableInformation charitableInfo)
        {
            if (!string.IsNullOrWhiteSpace(charitableInfo?.PicturePath))
            {
                FileInfo file = new FileInfo(charitableInfo.Photo01?.ImagePath);
                file.Delete();
            }

            if (!string.IsNullOrWhiteSpace(charitableInfo.Photo01?.ImagePath))
            {
                FileInfo file = new FileInfo(charitableInfo.Photo01.ImagePath);
                file.Delete();
            }

            if (!string.IsNullOrWhiteSpace(charitableInfo.Photo02?.ImagePath))
            {
                FileInfo file = new FileInfo(charitableInfo.Photo02.ImagePath);
                file.Delete();
            }
        }

        private static void deleteImage(string imageName, CharitableInformation charitableInfo)
        {          
            string path = string.Empty;

            switch (imageName)
            {
                case "picture":
                    path = charitableInfo?.PicturePath;
                    break;
                case "image01":
                    path = charitableInfo?.Photo01?.ImagePath;
                    break;
                case "image02":
                    path = charitableInfo?.Photo02?.ImagePath;
                    break;
            }
                      
            if (!string.IsNullOrWhiteSpace(path))
            {
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
        }

        private static void deleteImage(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
        }


        private async Task<Tuple<string, string>> UploadImage(IFormFile image, HttpRequest request, string filename = "")
        {
            if (image != null && image.Length > 0)
            {
                var folderName = Path.Combine("resources", "charities");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string fileName = string.Empty;

                if (string.IsNullOrWhiteSpace(filename))
                    fileName = string.Format($"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}_{image.FileName.Trim()}");
                else
                    fileName = filename;

                var imagePath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Tuple<string, string> data = new Tuple<string, string>(imagePath, string.Format($"{GetResourcesUri(request)}/{fileName}"));

                    await image.CopyToAsync(stream);

                    return data;
                }
            }

            return new Tuple<string, string>(string.Empty, string.Empty);
        }

        private string GetResourcesUri(HttpRequest request)
        {
            UriBuilder builder = new UriBuilder(request.Scheme, request.Host.Host)
            {
                Path = "resources/charities"
            };

            if (request.Host.Port.HasValue)
            {
                builder.Port = request.Host.Port.Value;
            }

            return builder.Uri.ToString();
        }
    }
}
