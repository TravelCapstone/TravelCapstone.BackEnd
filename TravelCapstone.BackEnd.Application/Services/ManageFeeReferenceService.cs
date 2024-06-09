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
    public class ManageFeeReferenceService:GenericBackendService, IManageFeeReferenceService
    {
        private readonly IRepository<ManagementFeeReference> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ManageFeeReferenceService(
            IRepository<ManagementFeeReference> repository,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetOperationFees(int Quantity)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                ManagementFeeResponse data = new ManagementFeeResponse();
                var organizationFee = await _repository.GetAllDataByExpression(m => m.ManagementFeeTypeId == Domain.Enum.ManagementFeeType.ORGANIZATION_COST && m.Moq <= Quantity, 0, 0, null, false, null);
                if(organizationFee.Items != null && organizationFee.Items.Count > 0)
                {
                    var suitableOrganizationFee = organizationFee.Items.OrderByDescending(m => m.Moq).FirstOrDefault();
                    data.MinOrganizationCost = suitableOrganizationFee.MinFee  * Quantity;
                    data.MaxOrganizationCost = suitableOrganizationFee.MaxFee * Quantity;
                } else
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy giá tham khảo phí tổ chức cho {Quantity} người");
                    return result;
                }

                var escortFee = await _repository.GetAllDataByExpression(m => m.ManagementFeeTypeId == Domain.Enum.ManagementFeeType.ESCORT_FEE && m.Moq <= Quantity, 0, 0, null, false, null);
                if (escortFee.Items != null && escortFee.Items.Count > 0)
                {
                    var suitableEscortFee = escortFee.Items.OrderByDescending(m => m.Moq).FirstOrDefault();
                    data.MinEscortFee = suitableEscortFee.MinFee * Quantity;
                    data.MaxEscortFee = suitableEscortFee.MaxFee * Quantity;
                }
                else
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy giá tham khảo phí escort cho {Quantity} người");
                    return result;
                }

                var operatingFee = await _repository.GetAllDataByExpression(m => m.ManagementFeeTypeId == Domain.Enum.ManagementFeeType.OPERATING_FEE && m.Moq <= Quantity, 0, 0, null, false, null);
                if (operatingFee.Items != null && operatingFee.Items.Count > 0)
                {
                    var suitableOperatingFee = operatingFee.Items.OrderByDescending(m => m.Moq).FirstOrDefault();
                    data.MinOperatingFee = suitableOperatingFee.MinFee * Quantity;
                    data.MaxOperatingFee = suitableOperatingFee.MaxFee * Quantity;
                }
                else
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy giá tham khảo công tác phí điều hành cho {Quantity} người");
                    return result;
                }

                var contingencyFee = await _repository.GetAllDataByExpression(m => m.ManagementFeeTypeId == Domain.Enum.ManagementFeeType.CONTINGENCY_FEE && m.Moq <= Quantity, 0, 0, null, false, null);
                if (contingencyFee.Items != null && contingencyFee.Items.Count > 0)
                {
                    var suitableContingencyFee = contingencyFee.Items.OrderByDescending(m => m.Moq).FirstOrDefault();
                    data.MinContingencyFee = suitableContingencyFee.MinFee * Quantity;
                    data.MaxContingencyFee = suitableContingencyFee.MaxFee * Quantity;
                }
                else
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy giá tham khảo công tác phí điều hành cho {Quantity} người");
                    return result;
                }

                result.Result = data;
            }catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
