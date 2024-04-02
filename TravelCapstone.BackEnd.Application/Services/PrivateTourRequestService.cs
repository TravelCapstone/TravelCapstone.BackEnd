using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class PrivateTourRequestService : GenericBackendService, IPrivateTourRequestService
    {
        private readonly IRepository<PrivateTourRequest> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrivateTourRequestService(
            IRepository<PrivateTourRequest> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO privateTourequestDTO)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                AppActionResult result = new AppActionResult();
                try
                {
                    var tourRepository = Resolve<IRepository<Tour>>();
                    var tourDb = await tourRepository.GetById(privateTourequestDTO.TourId);
                    if (tourDb == null)
                    {
                        result = BuildAppActionResultError(result, $"Tour with id {privateTourequestDTO.TourId} not found");
                        return result;
                    }

                    var accountRepository = Resolve<IRepository<Account>>();
                    var accountDb = await accountRepository.GetById(privateTourequestDTO.AccountId);
                    if (accountDb == null)
                    {
                        result = BuildAppActionResultError(result, $"Account with id {privateTourequestDTO.AccountId} not found");
                        return result;
                    }

                    //Need improvement for condition
                    if(!isValidTime(tourDb.EndDate, tourDb.StartDate, privateTourequestDTO.EndDate, privateTourequestDTO.StartDate))
                    {
                        result = BuildAppActionResultError(result, $"Request tour length is less than cloned one");
                        return result;
                    }

                    var request = _mapper.Map<PrivateTourRequest>(privateTourequestDTO);
                    request.Id = Guid.NewGuid();
                    request.Status = PrivateTourStatus.NEW;

                    await _repository.Insert(request);
                    await _unitOfWork.SaveChangesAsync();

                    result.Result = request;

                    if (!BuildAppActionResultIsError(result))
                    {
                        scope.Complete();
                    }
                } catch(Exception ex)
                {
                    result = BuildAppActionResultError(result, ex.Message);
                }
                return result;
            }
        }

        private bool isValidTime(DateTime cloneEnd, DateTime cloneStart, DateTime requestEnd, DateTime requestStart)
        {
            int[] cloneResult = GetDaysAndNights(cloneStart, cloneEnd);
            int[] requestResult = GetDaysAndNights(requestStart, requestEnd);
            return cloneResult[0] <= requestResult[0] && cloneResult[1] <= requestResult[1];
        }

        private int[] GetDaysAndNights(DateTime start, DateTime end)
        {
            int[] res = new int[2];
            DateTime curr = start;
            while(curr < end)
            {
                if (IsDayTime(curr))
                {
                    res[0]++;
                }
                else
                {
                    res[1]++;
                }
                curr = curr.AddHours(12);
            }
            return res;
        }

        private bool IsDayTime(DateTime start)
        {
            return (start.Hour >= 6 && start.Hour < 18);
        }

    }
}
