using AutoMapper;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Infrastructure.Persistence.Partners.MappingProfiles;

public class DomainToPersistenceProfile : Profile
{
    public DomainToPersistenceProfile()
    {
        CreateMap<PartnerRoleType, Models.PartnerRoleType>()
            .ReverseMap();
        CreateMap<Partner, Models.Partner>()
            .ForMember(dest => dest.IndividualFirstName,
                opt => opt.MapFrom(src => src.IndividualName.FirstName))
            .ForMember(dest => dest.IndividualLastName,
                opt => opt.MapFrom(src => src.IndividualName.LastName))
            .ReverseMap();
        CreateMap<PartnerBankAccount, Models.PartnerBankAccount>()
            .ReverseMap();
        CreateMap<PartnerContact, Models.PartnerContact>()
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.Name.LastName))
            .ReverseMap();
        CreateMap<PartnerIdentifier, Models.PartnerIdentifier>()
            .ReverseMap();
        CreateMap<Address, ValueObjectRecords.AddressRecord>()
            .ReverseMap();
        CreateMap<BankAccount, ValueObjectRecords.BankAccountRecord>()
            .ReverseMap();
    }
}
