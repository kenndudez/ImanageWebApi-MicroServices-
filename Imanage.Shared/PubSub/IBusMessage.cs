using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.PubSub
{
    public interface IBusMessage
    {
        int BusMessageType { get; set; }
    }
}
