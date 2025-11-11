using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAllowAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute));

        if (hasAllowAnonymous)
        {
            return;
        }
        
        var hasAuthorize = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .Any(m => m.GetType() == typeof(AuthorizeAttribute));

        if (hasAuthorize)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            
            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}