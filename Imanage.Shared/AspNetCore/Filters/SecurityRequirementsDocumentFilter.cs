using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.AspNetCore.Filters
{
    public class SecurityRequirementsDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument document, DocumentFilterContext context)
        {
            document.Security = new List<IDictionary<string, IEnumerable<string>>>() 
            {
                new Dictionary < string, IEnumerable < string >> () 
                { 
                    {"Bearer", new string[] {}},  
                    {"Basic", new string[] {}},
                }
            };
        }
    }
}
