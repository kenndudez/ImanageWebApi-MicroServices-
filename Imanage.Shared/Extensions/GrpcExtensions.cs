using System;
using System.IO;
using ProtoBuf;

namespace Imanage.Shared.Extensions
{
    public static class GrpcExtensions
    {
        public static byte[] SerializeToBytes(this object model)
        {

            byte[] data = new Byte[] { 0x0 };
            using (var file = new MemoryStream())
            {
                Serializer.Serialize(file, model);
                data = file.ToArray();
            }
            return  data;
        }

        public static T Deserialize<T>(this byte[] bytes)
        {
            if (bytes.Length <= 0)
                return default(T);

            using (var stream = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(stream);
            }
         }
    }
}
