
using Imanage.Shared.AspNetCore.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;

namespace Imanage.Shared.Extensions
{
    public static class SwaggerExtension
    {

        public static void UseCustomSwaggerApi(this IApplicationBuilder app, string name = "DAFMIS API V1")
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            //app.UseSwagger();

            app.UseSwagger(c => {
                c.RouteTemplate = "swagger/{documentname}/swagger.json";
            });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", name);
                c.DocExpansion(DocExpansion.None);
            });
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string title = "Dafmis")
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = title, Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.DocumentFilter<SecurityRequirementsDocumentFilter>();
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(security);
            });
            return services;
        }
    }
}
