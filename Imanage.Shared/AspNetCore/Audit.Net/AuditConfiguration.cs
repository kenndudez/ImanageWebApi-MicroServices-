using Audit.Core;
using Audit.WebApi;
using Imanage.Shared.AspNetCore.AuditNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.AspNetCore.AuditNet
{
    public static class AuditConfiguration
    {
        private const string CorrelationIdField = "CorrelationId";
        //public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// Add the global audit filter to the MVC pipeline
        /// </summary>
        public static MvcOptions AddAudit(this MvcOptions mvcOptions)
        {
            // Configure the global Action Filter
            mvcOptions.AddAuditFilter(a => a
                    .LogAllActions()
                    .WithEventType("MVC:{verb}:{controller}:{action}")
                    .IncludeModelState()
                    .IncludeRequestBody()
                    .IncludeResponseBody()
                    );
            return mvcOptions;
        }


        /// <summary>
        /// Global Audit configuration
        /// </summary>
        public static IServiceCollection ConfigureAudit(this IServiceCollection serviceCollection, string connectionString)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            Audit.Core.Configuration.Setup()
            //.UseSqlServer(config => config
            //    .ConnectionString(connectionString)
            //   .Schema("dbo")
            //    .TableName("Event")
            //    .IdColumnName("EventId")
            //    .JsonColumnName("JsonData")
            //    .LastUpdatedColumnName("LastUpdatedDate")
            //    .CustomColumn("EventType", ev => ev.EventType));
            .UseCustomProvider(new ImangeSerilogDataProvider(serviceProvider.GetRequiredService<IConfiguration>()));

            return serviceCollection;
        }

        //public static void UseAuditCorrelationId(this IApplicationBuilder app, IHttpContextAccessor ctxAccesor)
        //{

        //    Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
        //    {
        //        var httpContext = ctxAccesor.HttpContext;
        //        scope.Event.CustomFields[CorrelationIdField] = httpContext.TraceIdentifier;
        //    });
        //}

    }
}
