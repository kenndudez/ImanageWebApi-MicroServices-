using Audit.Core;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.AspNetCore.AuditNet
{
    public class ImangeSerilogDataProvider : AuditDataProvider
    {
        private readonly IConfiguration configuration;

        public ImangeSerilogDataProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private ILogger GetLogger(AuditEvent auditEvent)
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.LogstashHttp(configuration.GetValue<string>("Serilog:LogstashUrl") ?? "localhost://6000") //- uncomment to log to logstash
                 .CreateLogger();
        }

        private Microsoft.Extensions.Logging.LogLevel GetLogLevel(AuditEvent auditEvent)
        {
            return auditEvent.Environment.Exception != null ? Microsoft.Extensions.Logging.LogLevel.Error : Microsoft.Extensions.Logging.LogLevel.Information;
        }

        private void LogAudit(AuditEvent auditEvent, object eventId)
        {
            var logger = GetLogger(auditEvent);
            var level = GetLogLevel(auditEvent);
            var value = auditEvent.ToJson();
            switch (level)
            {
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    logger.Debug(value);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    logger.Warning(value);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    logger.Error(value);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    logger.Fatal(value);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                default:
                    logger.Information(value);
                    break;
            }
        }

        public override object InsertEvent(AuditEvent auditEvent)
        {
            var eventId = Guid.NewGuid();
            LogAudit(auditEvent, eventId);
            return eventId;
        }

        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            LogAudit(auditEvent, eventId);
        }
    }
}
