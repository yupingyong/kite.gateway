using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Application.Contracts.Dtos.ServiceGovernance;
using Volo.Abp.Domain.Repositories;
using Kite.Gateway.Domain.Entities;
using Mapster;

namespace Kite.Gateway.Application
{
    public class ServiceGovernanceAppService : BaseApplicationService, IServiceGovernanceAppService
    {
        private readonly IRepository<ServiceGovernanceConfigure> _repository;

        public ServiceGovernanceAppService(IRepository<ServiceGovernanceConfigure> repository)
        {
            _repository = repository;
        }

        public async Task<KiteResult<ServiceGovernanceConfigureDto>> GetServiceGovernanceConfigureAsync()
        {
            var result = (await _repository.GetQueryableAsync())
                .ProjectToType<ServiceGovernanceConfigureDto>()
                .FirstOrDefault();
            if (result == null)
            {
                result = new ServiceGovernanceConfigureDto();
            }
            return Ok(result);
        }

        public async Task<KiteResult> SaveServiceGovernanceConfigureAsync(ServiceGovernanceConfigureDto configure)
        {
            var model = await _repository.FirstOrDefaultAsync();
            if (model == null)
            {
                model = new ServiceGovernanceConfigure(GuidGenerator.Create());
                TypeAdapter.Adapt(configure, model);
                await _repository.InsertAsync(model);
            }
            else
            {
                TypeAdapter.Adapt(configure, model);
                await _repository.UpdateAsync(model);
            }
            return Ok();
        }
    }
}
