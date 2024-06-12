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
    public class ConfigurationService : GenericBackendService, IConfigurationService
    {
        private readonly IRepository<Configuration> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfigurationService(IServiceProvider serviceProvider,
            IRepository<Configuration> repository,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetMinQuantityForTourCreation(bool isForFamily)
        {
            AppActionResult result = new();
            try
            {
                if(isForFamily )
                {
                    var congfig = await _repository.GetByExpression(c => c.Name.Equals("MIN_QUANTITY_FOR_FAMILY"), null);
                    if(congfig != null )
                    {
                        result.Result = int.Parse(congfig.PreValue);
                    }
                }
                else
                {
                    var congfig = await _repository.GetByExpression(c => c.Name.Equals("MIN_QUANTITY_FOR_COMPANY"), null);
                    if (congfig != null)
                    {
                        result.Result = int.Parse(congfig.PreValue);
                    }
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result; 
        }
    }
}
