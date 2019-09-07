using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Service.TextAnalysis.Security
{
    internal sealed class SecurityRequirementDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", new string[] { } },
                    { "Basic", new string[] { } }
                }
            };
        }
    }
}