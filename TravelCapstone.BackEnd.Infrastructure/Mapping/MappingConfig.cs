using AutoMapper;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Infrastructure.Mapping;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Account, AccountResponse>()
                .ForMember(desc => desc.Id, act => act.MapFrom(src => src.Id))
                .ForMember(desc => desc.Email, act => act.MapFrom(src => src.Email))
                .ForMember(desc => desc.Gender, act => act.MapFrom(src => src.Gender))
                .ForMember(desc => desc.IsVerified, act => act.MapFrom(src => src.IsVerified))
                .ForMember(desc => desc.FirstName, act => act.MapFrom(src => src.FirstName))
                .ForMember(desc => desc.LastName, act => act.MapFrom(src => src.LastName))
                .ForMember(desc => desc.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber))
                .ForMember(desc => desc.UserName, act => act.MapFrom(src => src.UserName))
                ;
            
            config.CreateMap<PrivateTourRequest,PrivateTourRequestDto>()
                .ForMember(desc => desc.Id, act => act.MapFrom(src => src.Id))
                .ForMember(desc => desc.TourId, act => act.MapFrom(src => src.TourId))
                .ForMember(desc => desc.Description, act => act.MapFrom(src => src.Description))
                .ForMember(desc => desc.Name, act => act.MapFrom(src => src.Name))
                .ForMember(desc => desc.Status, act => act.MapFrom(src => src.Status))
                .ForMember(desc => desc.AccountId, act => act.MapFrom(src => src.AccountId))
                .ForMember(desc => desc.NumOfAdult, act => act.MapFrom(src => src.NumOfAdult))
                .ForMember(desc => desc.NumOfChildren, act => act.MapFrom(src => src.NumOfChildren))
                .ForMember(desc => desc.Account, act => act.MapFrom(src => src.Account));
            
            config.CreateMap<PagedResult<PrivateTourRequest>, PagedResult<PrivateTourRequestDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        });
        // Trong class MappingConfig

       

        return mappingConfig;
    }
}