using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;
using Mapster;

namespace Kite.Gateway.Domain.Node
{
    internal class NodeManager : DomainService, INodeManager
    {

        private readonly IRepository<Entities.Node> _repository;

        public NodeManager(IRepository<Entities.Node> repository)
        {
            _repository = repository;
        }

        public async Task<Entities.Node> CreateAsync(string nodeName, string server)
        {
            if (await _repository.AnyAsync(x => x.NodeName == nodeName))
            {
                throw new ArgumentException("节点名称已经存在");
            }
            if (await _repository.AnyAsync(x => x.Server == server))
            {
                throw new ArgumentException("节点服务器地址已经存在");
            }
            return new Entities.Node()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Description = "",
                NodeName = nodeName,
                Server = server,
                AccessToken = ""
            };
        }

        public async Task<Entities.Node> UpdateAsync(int id, string nodeName, string server)
        {
            if (await _repository.AnyAsync(x => x.NodeName == nodeName && x.Id != id))
            {
                throw new ArgumentException("节点名称已经存在");
            }
            if (await _repository.AnyAsync(x => x.Server == server && x.Id != id))
            {
                throw new ArgumentException("节点服务器地址已经存在");
            }
            var model= await _repository.FirstOrDefaultAsync(x => x.Id == id);
            model.Updated = DateTime.Now;
            return model;
        }
    }
}
