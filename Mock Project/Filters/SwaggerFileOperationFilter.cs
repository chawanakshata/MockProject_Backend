using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mock_Project.Filters
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var fileParameters = context.ApiDescription.ParameterDescriptions
                    .Where(p => p.ModelMetadata.ModelType == typeof(IFormFile) || p.ModelMetadata.ModelType == typeof(List<IFormFile>))
                    .ToList();

                if (fileParameters.Count > 0)
                {
                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = fileParameters.ToDictionary(
                                    p => p.Name,
                                    p => new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                ),
                                Required = fileParameters.Select(p => p.Name).ToHashSet()
                            }
                        }
                    }
                    };
                }
            }
        
    }
}
