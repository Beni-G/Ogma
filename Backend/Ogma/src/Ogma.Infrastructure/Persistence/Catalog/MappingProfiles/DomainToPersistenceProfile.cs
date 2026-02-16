using AutoMapper;
using Ogma.Application.Catalog.Dtos;
using Ogma.Domain.Catalog.Entities;

namespace Ogma.Infrastructure.Persistence.Catalog.MappingProfiles;
public class DomainToPersistenceProfile : Profile
{
    public DomainToPersistenceProfile()
    {
        CreateMap<Item, Models.Item>()
            .ForMember(dest => dest.ListPriceAmount,
                       opt => opt.MapFrom(src => src.ListPrice.Amount))
            .ForMember(dest => dest.ListPriceCurrency,
                       opt => opt.MapFrom(src => src.ListPrice.Currency))
            .ReverseMap();

        CreateMap<ItemDto, Models.Item>()
            .ForMember(dest => dest.ListPriceAmount,
                       opt => opt.MapFrom(src => src.ListPrice.Amount))
            .ForMember(dest => dest.ListPriceCurrency,
                       opt => opt.MapFrom(src => src.ListPrice.Currency))
            .ReverseMap();

        CreateMap<Category, Models.Category>().ReverseMap();

        CreateMap<CategoryDto, Models.Category>().ReverseMap();

        CreateMap<ItemType, Models.ItemType>().ReverseMap();

        CreateMap<ItemTypeDto, Models.ItemType>().ReverseMap();
    }
}
