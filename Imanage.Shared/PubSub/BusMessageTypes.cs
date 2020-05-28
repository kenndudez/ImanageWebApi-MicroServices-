using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.PubSub
{
    public enum BusMessageTypes
    {
        UNKNOWN,
        NEW_USER,
        EMAIL,
        NEW_MARKETER,
        NEW_MARKETER_ACCOUNT,
        NEW_TRUCK,
        USER_MSG,
        UPDATE_MARKETER_STATUS,
        WORKFLOW_PIPELINE,
        DOCUMENT_WORKFLOW_PIPELINE,
        NEW_TRUCK_OWNER,
        OUTLET_VERIFICATION,
        OUTLET_VERIFICATION_SCHEDULE_NOTIFICATION
    }
}