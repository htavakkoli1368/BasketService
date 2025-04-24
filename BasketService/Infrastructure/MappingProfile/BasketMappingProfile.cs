using AutoMapper;
using BasketService.Model.Entities;
using BasketService.Model.Services;

namespace BasketService.Infrastructure.MappingProfile
{
    public class BasketMappingProfile:Profile
    {

        public BasketMappingProfile()
        {
            CreateMap<BasketItems,AddItemsToBasketDto>().ReverseMap();
            CreateMap<Basket,BasketDto>().ReverseMap();
            CreateMap<BasketItems,BasketItemsDto>().ReverseMap();
        }
    }
}
