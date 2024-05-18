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
    public class EventService : GenericBackendService, IEventService
    {
        private readonly IRepository<Event> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IServiceProvider serviceProvider,
            IRepository<Event> repository,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppActionResult> GetEventListWithQuantity(int quantity)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var eventListDB = await _repository.GetAllDataByExpression(e => e.MOQ >= quantity, 0, 0, null, false, null);

                if(eventListDB.Items != null && eventListDB.Items.Count > 0)
                {
                    var eventIds = eventListDB.Items.Select(e => e.Id).ToList();
                    var eventDetailPriceHistoryRepository = Resolve<IRepository<EventDetailPriceHistory>>();
                    var eventPriceDb = await eventDetailPriceHistoryRepository!.GetAllDataByExpression(e => eventIds.Contains(e.EventDetail.EventId), 0, 0, null, false, e => e.EventDetail);
                    var eventpriceByEvent = eventPriceDb.Items
                        .GroupBy(e => e.EventDetail.EventId)
                        .ToDictionary(
                            g => g.Key, 
                            g => g
                            .GroupBy(g => g.EventDetailId)
                            .Select(s => s.
                                OrderByDescending(s => s.Date)
                                .FirstOrDefault()
                                )
                            .Where(p => p != null)
                            .ToList()
                        );
                    var data = new List<EventQuotationResponse>();
                    double total = 0;
                    foreach(var item in eventListDB.Items)
                    {
                        total = 0;
                        List<EventDetailReponse> eventDetailReponses = new List<EventDetailReponse>();
                        foreach(var detail in eventpriceByEvent[item.Id])
                        {
                            eventDetailReponses.Add(new EventDetailReponse
                            {
                                Name = detail.EventDetail.Name,
                                PerPerson = detail.EventDetail.PerPerson,
                                Price = detail.Price,
                                Quantity = detail.EventDetail.PerPerson? quantity : 1
                            });
                            total += detail.Price * (detail.EventDetail.PerPerson ? quantity : 1);
                        }
                        data.Add(new EventQuotationResponse()
                        {
                            Event = item,
                            EventDetailReponses = eventDetailReponses,
                            Total = total
                        });
                    }
                    result.Result = data;
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
