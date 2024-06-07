using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class TourguideAssignmentsService : GenericBackendService, ITourguideAssignmentsService
    {
        private readonly IRepository<TourguideAssignment> _tourguideAssignmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TourguideAssignmentsService(IServiceProvider serviceProvider, 
            IUnitOfWork unitOfWork,
            IRepository<TourguideAssignment> tourguideAssignmentRepository) : base(serviceProvider)
        {
            _tourguideAssignmentRepository = tourguideAssignmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetAvailableTourGuide(Guid provinceId, DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var accountRepository = Resolve<IRepository<Account>>();
                var provinceRepository = Resolve<IRepository<Province>>();
                var roleRepository = Resolve<IRepository<IdentityRole>>();
                var userRolesRepository = Resolve<IRepository<IdentityUserRole<string>>>();
                var tourGuideScope = Resolve<IRepository<TourguideScope>>();
                var tourGuideSalaryHistoryRepository = Resolve<IRepository<TourGuideSalaryHistory>>();
                var tourGuideList = new List<Account>();

                //get tourguide hoạt động trong province đó
                // get hoạt động assign của các tour guide đó
                // trả mấy ní rảnh
                var tourGuideScopeDb = await tourGuideScope!.GetAllDataByExpression(t => t.District.ProvinceId == provinceId, 0, 0, null, false, t => t.Account);
                var tourGuides = tourGuideScopeDb.Items!.DistinctBy(t => t.AccountId).Select(t => t.Account);
                var tourAssignmentRepository = Resolve<IRepository<TourguideAssignment>>(); //a < d && b > c
                var tourAssignmentDb = await tourAssignmentRepository!.GetAllDataByExpression(t => t.Tour.StartDate <= endDate && t.Tour.EndDate >= startDate, 0, 0, null, false, null);
                if(tourAssignmentDb.Items != null && tourAssignmentDb.Items.Count > 0)
                {
                    var busyTourGuide = tourAssignmentDb.Items.DistinctBy(a => a.AccountId).Select(a  => a.AccountId);
                    tourGuides = tourGuides.Where(t => !busyTourGuide.Contains(t.Id));
                }

                List<TourGuideSalaryHistory> tourGuideSalaryHistories = new List<TourGuideSalaryHistory>();
                foreach(var tourGuide in tourGuides)
                {
                    var tourGuideSalaryHistoryDb = await tourGuideSalaryHistoryRepository!.GetAllDataByExpression(t => t.AccountId == tourGuide!.Id, 0,0, null, false, t => t.Account!);
                    if(tourGuideSalaryHistoryDb.Items != null && tourGuideSalaryHistoryDb.Items.Count > 0)
                    {
                        tourGuideSalaryHistories.Add(tourGuideSalaryHistoryDb.Items.OrderByDescending(t => t.Date).FirstOrDefault()!);
                    }
                }
                    result.Result = new PagedResult<TourGuideSalaryHistory>
                    {
                        Items = tourGuideSalaryHistories.ToList()!,
                        TotalPages = (tourGuideSalaryHistories.Count() % pageSize == 0) ? tourGuideSalaryHistories.Count() / pageSize : tourGuideSalaryHistories.Count() / pageSize + 1
                    };


            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        private double CalculateDistance(double? lat1, double? lon1, double? lat2, double? lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
            {
                throw new ArgumentException("Latitude or longitude values are null.");
            }

            // Unwrap nullable double values to non-nullable doubles
            double latitude1 = lat1.Value;
            double longitude1 = lon1.Value;
            double latitude2 = lat2.Value;
            double longitude2 = lon2.Value;

            // Haversine formula to calculate distance between two points on Earth
            double R = 6371; // Earth radius in kilometers
            double dLat = ToRadians(latitude2 - latitude1);
            double dLon = ToRadians(longitude2 - longitude1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(latitude1)) * Math.Cos(ToRadians(latitude2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return distance;
        }
        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public async Task<AppActionResult> GetUnassignTourGuideByProvince(Guid provinceId)
        {
            var result = new AppActionResult();
            try
            {
                var tourtourGuidesRepository = Resolve<IRepository<TourguideAssignment>>();
                var tourtouGuideList = await tourtourGuidesRepository!.GetAllDataByExpression(p => p.ProvinceId == provinceId, 0, 0, null, true, p => p.Account!);
                if (tourtouGuideList == null)
                {
                    return result;
                }
                var assignedTourguide = tourtouGuideList!.Items!.Select(p => p.AccountId);
                result.Result = await _tourguideAssignmentRepository.GetAllDataByExpression(p => !assignedTourguide.Contains(p.AccountId), 0, 0, null, true, p => p.Account!, p => p.Province!);
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;  
        }

        public async Task<AppActionResult> GetMaxTourGuideNumber(int numOfVehicle)
        {
            var result = new AppActionResult();
            try
            {
                var preValueRepository = Resolve<IRepository<Configuration>>();
                var preValueDb = await preValueRepository!.GetByExpression(p =>  p.Name == SD.ConfigName.CONFIG_NAME);
                if (preValueDb == null)
                {
                    result = BuildAppActionResultError(result, "Config này không tồn tại");
                }
                var numberMatch = Regex.Match(preValueDb!.PreValue, @"\d+");
                if (!numberMatch.Success)
                {
                    result = BuildAppActionResultError(result, "No number found in preValue");
                    return result;
                }
                int extractedNumber = int.Parse(numberMatch.Value);

                result.Result = numOfVehicle + extractedNumber; 
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
