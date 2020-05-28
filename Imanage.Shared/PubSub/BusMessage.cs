using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using ProtoBuf;

namespace Imanage.Shared.PubSub
{
    [ProtoContract]
    public class BusMessage
    {
        [ProtoMember(1)]
        public int BusMessageType { get; set; }
        [ProtoMember(2)]
        public string Data { get; set; }

    }
}
