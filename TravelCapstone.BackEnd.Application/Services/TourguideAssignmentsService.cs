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
                var tourGuideList = new List<Account>();
                var provinceDb = await provinceRepository!.GetById(provinceId);
                if (provinceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy thông tin địa điểm bắt đầu với id {provinceId}");
                    return result;
                }
                var provinceList = await provinceRepository.GetAllDataByExpression(null, 0, 0, null, false, null);
                if (provinceList.Items != null && provinceList.Items.Count > 0)
                {
                    double shortestDistance = double.MaxValue;
                    List<Province> closestProvinces = new List<Province>();

                    foreach (var item in provinceList.Items)
                    {
                        double distance = CalculateDistance(provinceDb.lat, provinceDb.lng, item.lat, item.lng);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            closestProvinces.Add(item);
                        }
                    }
                    var closestProvinceIds = closestProvinces.Select(p => p.Id).ToList();
                    var tourtourGuideAssignmentRepository = Resolve<IRepository<TourguideAssignment>>();

                    foreach (var id in closestProvinceIds)
                    {
                        var tourguideAssignmentList = await tourtourGuideAssignmentRepository!.GetAllDataByExpression(
                            p => p.ProvinceId == provinceId && (p.Tour!.EndDate >= startDate || p.Tour.StartDate <= endDate),
                            0, 0, null, true, p => p.Account!, p => p.Province!
                        );

                        if (tourguideAssignmentList.Items != null && tourguideAssignmentList.Items.Count > 0)
                        {
                            var assignedTourguide = tourguideAssignmentList!.Items!.Select(p => p.AccountId).ToList();
                            var anotherTourguideInProvince = await tourGuideScope!.GetAllDataByExpression(p => !assignedTourguide.Contains(p.AccountId) && p.District!.ProvinceId == id, 0, 0, null, false, null);

                            var anotherTourguideInProvinceIds = anotherTourguideInProvince!.Items!.Select(p => p.AccountId).ToList();

                            // Get tour guide role ID
                            var tourGuideRole = await roleRepository!.GetAllDataByExpression(r => r.Name == "TOUR GUIDE", 0, 0, null, true, null);
                            var tourGuideRoleId = tourGuideRole!.Items!.FirstOrDefault()?.Id;
                            // Get all user roles
                            var userRoles = await userRolesRepository!.GetAllDataByExpression(ur => ur.RoleId == tourGuideRoleId && anotherTourguideInProvinceIds.Contains(ur.UserId), 0, 0, null, true, null);
                            var availableTourGuideIds = userRoles.Items!.Select(ur => ur.UserId).ToList();

                            var tourguideDb = await accountRepository!.GetByExpression(
                                p => availableTourGuideIds.Contains(p.Id)
                            );
                            if (tourguideDb != null)
                            {
                                tourGuideList.Add(tourguideDb);
                            }
                            // Assuming you need to process or return tourguideDb, do it here.

                            result.Result = new PagedResult<Account>
                            {
                                Items = tourGuideList,
                                TotalPages = anotherTourguideInProvince.TotalPages,
                            };
                        }
                    }
                }
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
