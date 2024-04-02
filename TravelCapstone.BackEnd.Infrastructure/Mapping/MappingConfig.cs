﻿using AutoMapper;
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

            config.CreateMap<PrivateTourRequestDTO, PrivateTourRequest>()
                .ForMember(desc => desc.Name, act => act.MapFrom(src => src.Name))
                .ForMember(desc => desc.StartDate, act => act.MapFrom(src => src.StartDate))
                .ForMember(desc => desc.EndDate, act => act.MapFrom(src => src.EndDate))
                .ForMember(desc => desc.Description, act => act.MapFrom(src => src.Description))
                .ForMember(desc => desc.NumOfAdult, act => act.MapFrom(src => src.NumOfAdult))
                .ForMember(desc => desc.NumOfChildren, act => act.MapFrom(src => src.NumOfChildren))
                .ForMember(desc => desc.TourId, act => act.MapFrom(src => src.TourId))
                .ForMember(desc => desc.MainVehicle, act => act.MapFrom(src => src.MainVehicle))
                .ForMember(desc => desc.isEnterprise, act => act.MapFrom(src => src.isEnterprise))
                .ForMember(desc => desc.AccountId, act => act.MapFrom(src => src.AccountId))
                ;
        });
        return mappingConfig;
    }
}