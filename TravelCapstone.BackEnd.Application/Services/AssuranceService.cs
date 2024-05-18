using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class AssuranceService : GenericBackendService, IAssuranceService
    {
        private readonly IRepository<Assurance> _repository;
        private readonly IRepository<AssurancePriceHistory> _historyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssuranceService(IServiceProvider serviceProvider,
            IRepository<Assurance> repository,
            IRepository<AssurancePriceHistory> historyRepository,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppActionResult> GetAvailableAssuranceList(int NumOfDay)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var assurancePriceDb = await _historyRepository.GetAllDataByExpression(h => h.Assurance!.DayMOQ >= NumOfDay, 0, 0, null, false, h => h.Assurance!);
                var lastestPrice = assurancePriceDb.Items!.GroupBy(a => a.Id).Select(a => a.OrderByDescending(b => b.Date)).ToList();
                if(lastestPrice.Count > 0)
                {
                    result.Result = lastestPrice;
                }
                else
                {
                    result.Result = null;
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
