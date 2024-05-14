using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebAPI_Demo.Extensions
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureNamedOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo()
                {
                    Title = Assembly.GetCallingAssembly().GetName().Name,
                    Version = "v1",
                };

                if (description.IsDeprecated) info.Description += "This API version has been deprecated.";

                options.SwaggerDoc(description.GroupName, info);
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }


        public class RemoveVersionParameterFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "api-version");
                if (versionParameter != null)
                {
                    operation.Parameters.Remove(versionParameter);
                }
            }
        }

        public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = new OpenApiPaths();
                foreach (var path in swaggerDoc.Paths)
                {
                    paths.Add(path.Key.Replace("{version}", swaggerDoc.Info.Version), path.Value);
                }
                swaggerDoc.Paths = paths;
            }
        }
    }
}
