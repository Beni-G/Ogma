using AutoMapper;
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
            .ForMember(dest => dest.CategoryId,
                       opt => opt.MapFrom(src => src.Category.Id))
            .ForMember(dest => dest.ItemTypeId,
                       opt => opt.MapFrom(src => src.ItemType.Id))
            .ReverseMap();

        CreateMap<Category, Models.Category>().ReverseMap();

        CreateMap<ItemType, Models.ItemType>().ReverseMap();
    }
}
