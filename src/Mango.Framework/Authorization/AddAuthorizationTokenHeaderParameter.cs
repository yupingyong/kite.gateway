using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mango.Framework.Authorization
{
    public class AddAuthorizationTokenHeaderParameter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            
            operation.Parameters.Add(new OpenApiParameter()
            {
                In = ParameterLocation.Header,
                Required = true,
                Name = "Authorization",
                Description= "AuthorizationToken"
            });
        }
    }
}
