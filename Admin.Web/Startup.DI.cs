using Auth.Core.Context;
using Auth.Core.EventHandlers;
using Auth.Core.Services;
using Auth.Core.Services.Interfaces;
using Imanage.Shared;
using Imanage.Shared.AspNetCore.Filters;
using Imanage.Shared.AspNetCore.Policy;
using Imanage.Shared.EF;
using Imanage.Shared.EF.Repository;
using Imanage.Shared.Net.WorkerService;
using Imanage.Shared.PubSub;
using Imanage.Shared.PubSub.KafkaImpl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Web
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            //Permission handler
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
           
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILandLordUserService, LandLordUserService>();
            services.AddTransient<IEstateManagerUserService, EstateManagerUserService>();

            services.AddSingleton<IProducerClient<BusMessage>>(service =>
            {
                var topics = Configuration.GetSection("Kafka").GetValue<string>("Topics").ToString().Split(",");
                var env = service.GetRequiredService<IHostingEnvironment>();
                var producerClient = new ProducerClient<BusMessage>(env, Configuration);
                return producerClient;
            });

            services.AddSingleton<IConsumerClient<BusMessage>>(service =>
            {
                var topics = Configuration.GetSection("Kafka").GetValue<string>("Topics").ToString().Split(",");
                var env = service.GetRequiredService<IHostingEnvironment>();
                var consumerClient = new ConsumerClient<BusMessage>(env, Configuration);
                //consumerClient.Subscribe(topics.ToList());
                return consumerClient;
            });

            services.AddTransient<Func<List<BusHandler>>>(cont =>
            () =>
            {
                List<BusHandler> handlers = new List<BusHandler>();
                var scope = cont.GetRequiredService<IServiceProvider>().CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<AuthHandler>();

                handlers.Add((message) => {
                    if (message.BusMessageType == (int)BusMessageTypes.NEW_USER)
                    {
                        handler.HandleCreateMarketerUser(message);
                        handler.HandleCreateTruckOwnerUser(message);
                    }
                });

                return handlers;
            });

            services.AddMediatR(typeof(Startup));
            //services.AddTransient(p => BuildMediator());
            services.AddScoped<ImanageExceptionAttribute>();
            services.AddSingleton<BoundedMessageChannel<BusMessage>>();
            services.AddHostedService<EvenHubProcessorService>();
            services.AddHostedService<EventHubReaderService>();
            services.AddScoped<ImanageExceptionAttribute>();
            services.AddScoped<IDbContext, ImanageAuthDbContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddTransient<AuthHandler>();

        }
    }
}
