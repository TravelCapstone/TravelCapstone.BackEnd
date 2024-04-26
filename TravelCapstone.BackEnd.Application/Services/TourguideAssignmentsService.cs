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

        public async Task<AppActionResult> GetUnassignTourGuideByProvince(Guid provinceId)
        {
            var result = new AppActionResult();
            try
            {
                var tourtourGuidesRepository = Resolve<IRepository<TourTourguide>>();
                var tourtouGuideList = await tourtourGuidesRepository!.GetAllDataByExpression(p => p.TourguideAssignment!.ProvinceId == provinceId, 0, 0, null, true, p => p.TourguideAssignment!);
                if (tourtouGuideList == null)
                {
                    return result;  
                }
                var assignedTourguide = tourtouGuideList!.Items!.Select(p => p.TourguideAssignmentId);
                result.Result = await _tourguideAssignmentRepository.GetAllDataByExpression(p => !assignedTourguide.Contains(p.Id), 0 , 0 , null, true, p => p.Account!, p => p.Province!);
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;  
        }
    }
}
