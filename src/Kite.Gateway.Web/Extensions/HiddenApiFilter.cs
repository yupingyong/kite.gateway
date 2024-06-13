using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kite.Gateway.Management.Web.Extensions
{
    /// <summary>
    /// 隐藏Abp自带的接口以及实体映射文档信息
    /// </summary>
    public class HiddenApiFilter: IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
           
            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    foreach (var res in operation.Value.Responses)
                    {
                        if (res.Key!="200")
                            operation.Value.Responses.Remove(res.Key);
                    }
                }
                if (path.Key.Contains("abp"))
                    swaggerDoc.Paths.Remove(path.Key);
                
            }
            foreach (var schema in context.SchemaRepository.Schemas)
            {
                if(schema.Key.ToLower().Contains("abp"))
                    context.SchemaRepository.Schemas.Remove(schema.Key);
            }
        }
    }
}
