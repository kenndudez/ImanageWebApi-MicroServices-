using Auth.Core.Services.Interfaces;
using Imanage.Shared.DI;
using Imanage.Shared.Enums;
using Imanage.Shared.EventModels;
using Imanage.Shared.PubSub;
using Imanage.Shared.ViewModels.MarketerSharedModels;
using Imanage.Shared.ViewModels.TruckOwnerSharedModels;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auth.Core.EventHandlers
{
    public class AuthHandler : INotificationHandler<SetupUserEvent>
    {
        private readonly ILandLordUserService _marketerUserService;
        private readonly IEstateManagerUserService _truckOwnerUserService;

        public AuthHandler(/*IMarketerUserService marketerUserService*/)
        {
            _marketerUserService = ServiceLocator.Current.GetInstance<ILandLordUserService>();// marketerUserService;
            _truckOwnerUserService = ServiceLocator.Current.GetInstance<IEstateManagerUserService>();
        }

        public Task Handle(SetupUserEvent notification, CancellationToken cancellationToken)
        {
            var marketerUserInfo = JsonConvert.DeserializeObject<LandLordSetUpUser>(notification.Message);
            var result = _marketerUserService.CreateMarketerUser(marketerUserInfo);

            return Task.FromResult(0);
        }

        public void HandleCreateMarketerUser(BusMessage message)
        {
            var landlordUserInfo = JsonConvert.DeserializeObject<LandLordSetUpUser>(message.Data);
            if (landlordUserInfo.UserType == (int)UserTypes.Estate_Manager)
            {
                _marketerUserService.CreateMarketerUser(landlordUserInfo);

            }
        }

        public void HandleCreateTruckOwnerUser(BusMessage message)
        {
            var truckOwnerUserInfo = JsonConvert.DeserializeObject<EstateManagerSetUpUser>(message.Data);
            if(truckOwnerUserInfo.UserType == (int)UserTypes.Estate_Manager)
            {
                _truckOwnerUserService.CreateTruckOwnerUser(truckOwnerUserInfo);
            }
        }
    }
}
