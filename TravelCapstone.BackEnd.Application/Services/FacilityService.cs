﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class FacilityService : GenericBackendService, IFacilityService
    {
        private readonly IMapper _mapper;
        private IRepository<Facility> _repository;
        private IUnitOfWork _unitOfWork;

        public FacilityService(
            IRepository<Facility> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppActionResult> GetAllFacility(int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
               result.Result = await _repository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllFacilityByRatingId(FilterLocation filter, Rating ratingId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.District!.ProvinceId == filter.ProvinceId &&  a.FacilityRating!.RatingId == ratingId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                if (filter.DistrictId != null && filter.CommuneId == null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.DistrictId == filter.DistrictId && a.FacilityRating!.RatingId == ratingId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);

                }
                else if (filter.DistrictId != null && filter.CommuneId != null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.CommunceId == filter.CommuneId && a.FacilityRating!.RatingId == ratingId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);

                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetFacilityByProvinceId(FilterLocation filter, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                //var communeRepository = Resolve<IRepository<Commune>>();
                //var communeDb = await communeRepository!.GetAllDataByExpression(c => c.District!.ProvinceId == provinceId, 0, 0, null, false, null);
                //if(communeDb.Items != null & communeDb.Items!.Count > 0)
                //{
                //    var communeIds = communeDb.Items!.Select(c => c.Id);
                //    var facilityDb = await _repository.GetAllDataByExpression(f => communeIds.Contains(f.CommunceId), pageNumber, pageSize, null, false, a=> a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                //    result.Result = facilityDb;
                //}
                result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.District!.ProvinceId == filter.ProvinceId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                if(filter.DistrictId != null && filter.CommuneId == null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.DistrictId == filter.DistrictId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);

                }
                else if(filter.DistrictId !=null && filter.CommuneId != null) 
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.CommunceId == filter.CommuneId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);

                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
