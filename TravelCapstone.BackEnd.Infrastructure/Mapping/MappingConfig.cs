using AutoMapper;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
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

            config.CreateMap<PrivateTourRequestDTO, PrivateTourRequest>()
                .ForMember(desc => desc.StartDate, act => act.MapFrom(src => src.StartDate))
                .ForMember(desc => desc.EndDate, act => act.MapFrom(src => src.EndDate))
                .ForMember(desc => desc.Description, act => act.MapFrom(src => src.Description))
                .ForMember(desc => desc.NumOfAdult, act => act.MapFrom(src => src.NumOfAdult))
                .ForMember(desc => desc.NumOfChildren, act => act.MapFrom(src => src.NumOfChildren))
                .ForMember(desc => desc.TourId, act => act.MapFrom(src => src.TourId))
                .ForMember(desc => desc.IsEnterprise, act => act.MapFrom(src => src.IsEnterprise))
                .ForMember(desc => desc.Note, act => act.MapFrom(src => src.Note))
                .ForMember(desc => desc.RecommendedTourUrl, act => act.MapFrom(src => src.RecommnendedTourUrl))
                .ForMember(desc => desc.StartLocation, act => act.MapFrom(src => src.StartLocation))
                .ForMember(desc => desc.StartLocationCommuneId, act => act.MapFrom(src => src.StartCommuneId))
                .ForMember(desc => desc.MainDestinationId, act => act.MapFrom(src => src.MainDestinationId))
                .ForMember(desc => desc.CreateBy, act => act.MapFrom(src => src.AccountId))
                
                .ReverseMap()
                ;

            config.CreateMap<PrivateTourRequest, PrivateTourResponeDto>()
                .ForMember(desc => desc.StartDate, act => act.MapFrom(src => src.StartDate))
                .ForMember(desc => desc.EndDate, act => act.MapFrom(src => src.EndDate))
                .ForMember(desc => desc.CreateDate, act => act.MapFrom(src => src.CreateDate))
                .ForMember(desc => desc.Description, act => act.MapFrom(src => src.Description))
                .ForMember(desc => desc.NumOfAdult, act => act.MapFrom(src => src.NumOfAdult))
                .ForMember(desc => desc.NumOfChildren, act => act.MapFrom(src => src.NumOfChildren))
                .ForMember(desc => desc.TourId, act => act.MapFrom(src => src.TourId))
                .ForMember(desc => desc.IsEnterprise, act => act.MapFrom(src => src.IsEnterprise))
                .ForMember(desc => desc.Note, act => act.MapFrom(src => src.Note))
                .ForMember(desc => desc.RecommnendedTourUrl, act => act.MapFrom(src => src.RecommendedTourUrl))
                .ForMember(desc => desc.MainDestination, act => act.MapFrom(src => src.Province))
                .ForMember(desc => desc.Status, act => act.MapFrom(src => src.PrivateTourStatus))
                .ForMember(desc => desc.StartLocation, act => act.MapFrom(src => src.StartLocation))
                .ForMember(desc => desc.AccountId, act => act.MapFrom(src => src.CreateBy))
                .ForMember(desc => desc.Account, act => act.MapFrom(src => src.CreateByAccount))
                .ForMember(desc => desc.Name, act => act.MapFrom(src => $"{src.CreateByAccount!.FirstName} {src.CreateByAccount.LastName}"))
                ;


            config.CreateMap<Customer,CustomerDto>()
                 .ForMember(desc => desc.Id, act => act.MapFrom(src => src.Id))
                 .ForMember(desc => desc.LastName, act => act.MapFrom(src => src.LastName))
                 .ForMember(desc => desc.FirstName, act => act.MapFrom(src => src.FirstName))
                 .ForMember(desc => desc.Email, act => act.MapFrom(src => src.Email))
                 .ForMember(desc => desc.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber))
                 .ForMember(desc => desc.AccountId, act => act.MapFrom(src => src.AccountId))
                 .ForMember(desc => desc.Address, act => act.MapFrom(src => src.Address))
                 .ForMember(desc => desc.Dob, act => act.MapFrom(src => src.Dob))
                 .ForMember(desc => desc.Gender, act => act.MapFrom(src => src.Gender))
                 .ForMember(desc => desc.IsAdult, act => act.MapFrom(src => src.IsAdult))
                 .ForMember(desc => desc.Money, act => act.MapFrom(src => src.Money))
                 .ForMember(desc => desc.Account, act => act.MapFrom(src => src.Account))
                 .ForMember(desc => desc.IsVerfiedNPhoneNumber, act => act.MapFrom(src => src.IsVerfiedPhoneNumber))
                 .ForMember(desc => desc.IsVerifiedEmail, act => act.MapFrom(src => src.IsVerifiedEmail))
                 .ReverseMap()
                 ;


            config.CreateMap<Customer, TourRegistrationDto>()
                 .ForMember(desc => desc.Id, act => act.MapFrom(src => src.Id))
                 .ForMember(desc => desc.LastName, act => act.MapFrom(src => src.LastName))
                 .ForMember(desc => desc.FirstName, act => act.MapFrom(src => src.FirstName))
                 .ForMember(desc => desc.Email, act => act.MapFrom(src => src.Email))
                 .ForMember(desc => desc.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber))
                 .ForMember(desc => desc.Address, act => act.MapFrom(src => src.Address))
                 .ForMember(desc => desc.Dob, act => act.MapFrom(src => src.Dob))
                 .ForMember(desc => desc.Gender, act => act.MapFrom(src => src.Gender))
                 .ForMember(desc => desc.IsAdult, act => act.MapFrom(src => src.IsAdult))
                 ;

            config.CreateMap<PagedResult<CustomerDto>, PagedResult<Customer>>()
             .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            config.CreateMap<PagedResult<PrivateTourRequest>, PagedResult<PrivateTourResponeDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        });
        // Trong class MappingConfig

        return mappingConfig;
    }
}