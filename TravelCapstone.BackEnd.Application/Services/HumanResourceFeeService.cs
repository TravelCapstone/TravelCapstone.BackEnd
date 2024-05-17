using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class HumanResourceFeeService : GenericBackendService, IHumanResourceFeeService
    {
        private readonly IRepository<TourGuideSalaryHistory> _tourGuideSalaryHistoryRepository;
        private readonly IRepository<DriverSalaryHistory> _driverSalaryHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HumanResourceFeeService(IServiceProvider serviceProvider,
            IRepository<TourGuideSalaryHistory> tourGuideSalaryHistoryRepository,
            IRepository<DriverSalaryHistory> driverSalaryHistoryRepository,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _tourGuideSalaryHistoryRepository = tourGuideSalaryHistoryRepository;
            _driverSalaryHistoryRepository = driverSalaryHistoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppActionResult> GetSalary(List<HumanResourceCost> dtos, bool IsForTourguide)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                if(dtos.Count > 0)
                {
                    double total = 0;
                    double baseSalary = 0;
                    if(IsForTourguide)
                    {
                        var tourguideSalary = await _tourGuideSalaryHistoryRepository.GetAllDataByExpression(null, 0, 0, null, false, null);
                        var latestTourguideSalary = tourguideSalary.Items!.GroupBy(s => s.AccountId).Select(s => s.OrderByDescending(s => s.Date).FirstOrDefault()).ToList();
                        baseSalary = latestTourguideSalary.Sum(i => i.Salary) / latestTourguideSalary.Count();
                    } else
                    {
                        var driverSalary = await _driverSalaryHistoryRepository.GetAllDataByExpression(null, 0, 0, null, false, null);
                        baseSalary = driverSalary.Items!.Sum(i => i.Salary) / driverSalary.Items!.Count();
                    }
                    dtos.ForEach(d => total += d.NumOfDay * d.Quantity * baseSalary);
                    result.Result = total;
                }
                else
                {
                    result.Result = 0;
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;

        }
    }
}
