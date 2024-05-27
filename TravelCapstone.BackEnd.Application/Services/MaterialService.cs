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
    public class MaterialService : GenericBackendService, IMaterialService
    {
        private readonly IRepository<Material> _repository;
        private readonly IRepository<MaterialAssignment> _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IServiceProvider serviceProvider,
            IRepository<Material> repository,
        IRepository<MaterialAssignment> assignmentRepository,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _assignmentRepository = assignmentRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> AddMaterialtoTour(AddMaterialRequest request)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var tourRepository = Resolve<IRepository<Tour>>();
                var tourDb = await tourRepository!.GetById(request.TourId);
                if (tourDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy tour với id {request.TourId}");
                    return result;
                }
                List<MaterialAssignment> materialAssignments = new List<MaterialAssignment>();
                foreach(var item in request.MaterialRequests)
                {
                    materialAssignments.Add(new MaterialAssignment()
                    {
                        Id = Guid.NewGuid(),
                        MaterialPriceHistoryId = item.MaterialSellPriceId,
                        Quantity = item.Quantity,
                        TourId = tourDb.Id
                    });
                }
                await _assignmentRepository.InsertRange(materialAssignments);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetLatestMaterialListPrice()
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var materialListDB = await _repository.GetAllDataByExpression(null, 0, 0, null, false, null);

                if (materialListDB.Items != null && materialListDB.Items.Count > 0)
                {

                    var materialIds = materialListDB.Items.Select(e => e.Id).ToList();
                    var materialDetailPriceHistoryRepository = Resolve<IRepository<MaterialPriceHistory>>();
                    var materialPriceDb = await materialDetailPriceHistoryRepository!.GetAllDataByExpression(e => materialIds.Contains(e.MaterialId), 0, 0, null, false, e => e.Material);
                    var latestMaterialPrice = materialPriceDb.Items
                        .GroupBy(e => e.MaterialId)
                        .Select(s => s.OrderByDescending(d => d.Date).FirstOrDefault()).ToList();
                   
                    result.Result = latestMaterialPrice;
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetMaterialByMaterialType(MaterialType type)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var materialDb = await _repository.GetAllDataByExpression(m => m.MaterialTypeId == type, 0, 0, null, false, null);
                result.Result = materialDb;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
