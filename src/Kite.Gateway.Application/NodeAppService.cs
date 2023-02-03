using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Application.Contracts.Dtos.Node;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Kite.Gateway.Application
{
    public class NodeAppService : BaseApplicationService, INodeAppService
    {
        private readonly IRepository<Node> _repository;
        private readonly INodeManager _nodeManager;

        public NodeAppService(IRepository<Node> repository, INodeManager nodeManager)
        {
            _repository = repository;
            _nodeManager = nodeManager;
        }

        public async Task<KiteResult> CreateAsync(CreateNodeDto createNode)
        {
            createNode.Server = createNode.Server.TrimEnd('/');
            var model =await _nodeManager.CreateAsync(createNode.NodeName, createNode.Server);
            TypeAdapter.Adapt(createNode, model);
            await _repository.InsertAsync(model);
            return Ok();
        }

        public async Task<KiteResult> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(x => x.Id == id);
            return Ok();
        }

        public async Task<KiteResult<List<NodeDto>>> GetAsync()
        {
            var result = (await _repository.GetQueryableAsync())
                .OrderByDescending(x => x.Created)
                .ProjectToType<NodeDto>()
                .ToList();
            return Ok(result);
        }

        public async Task<KiteResult<NodeDto>> GetAsync(int id)
        {
            var result= (await _repository.GetQueryableAsync())
                .Where(x => x.Id == id)
                .ProjectToType<NodeDto>()
                .FirstOrDefault();
            return Ok(result);
        }
        public async Task<KitePageResult<List<NodeDto>>> GetNodesAsync(int page = 1, int pageSize = 10)
        {
            var query = await _repository.GetQueryableAsync();
            var result = query
                .OrderByDescending(x => x.Created)
                .PageBy((page - 1) * pageSize, pageSize)
                .ProjectToType<NodeDto>()
                .ToList();
            return Ok(result, query.Count());
        }

        public async Task<KiteResult> UpdateAsync(UpdateNodeDto updateNode)
        {
            updateNode.Server = updateNode.Server.TrimEnd('/');
            var model =await _nodeManager.UpdateAsync(updateNode.Id, updateNode.NodeName, updateNode.Server);
            TypeAdapter.Adapt(updateNode, model);
            model.Updated = DateTime.Now;
            await _repository.UpdateAsync(model);
            return Ok();
        }
    }
}
