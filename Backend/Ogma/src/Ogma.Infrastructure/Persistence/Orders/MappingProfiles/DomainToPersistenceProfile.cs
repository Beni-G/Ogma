using AutoMapper;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Infrastructure.Persistence.Orders.MappingProfiles;

public class DomainToPersistenceProfile : Profile
{
    public DomainToPersistenceProfile()
    {
        CreateMap<OrderPartner, ValueObjectRecords.OrderPartner>()
            .ForMember(dest => dest.PartnerId,
                       opt => opt.MapFrom(src => src.PartnerId))
            .ForMember(dest => dest.PartnerName,
                       opt => opt.MapFrom(src => src.PartnerName))
            .ReverseMap();
        CreateMap<OrderItem, ValueObjectRecords.OrderItem>()
            .ForMember(dest => dest.ItemId,
                       opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.ItemName,
                       opt => opt.MapFrom(src => src.ItemName))
            .ForMember(dest => dest.ItemCode,
                       opt => opt.MapFrom(src => src.ItemCode))
            .ReverseMap();
        CreateMap<OrderType, Models.OrderType>()
            .ReverseMap();
        CreateMap<OrderStatus, Models.OrderStatus>()
            .ReverseMap();
        CreateMap<OrderLine, Models.OrderLine>()
            .ForMember(d => d.PriceAmount,
                       opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(d => d.PriceCurrency,
                       opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(d => d.ExchangeRate,
                opt => opt.MapFrom(s => GetExchangeRate(s.ExchangeRate)))
            .ForMember(d => d.ExchangeTargetCurrency,
                opt => opt.MapFrom(s => GetExchangeTargetCurrency(s.ExchangeRate)))
            .ForMember(d => d.Order, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<Order, Models.Order>()
            .ReverseMap();
    }

    private static decimal GetExchangeRate(ExchangeRate? exchangeRate)
    {
        return exchangeRate?.Rate ?? 1m;
    }

    private static string GetExchangeTargetCurrency(ExchangeRate? exchangeRate)
    {
        return exchangeRate?.TargetCurrency ?? string.Empty;
    }
}
