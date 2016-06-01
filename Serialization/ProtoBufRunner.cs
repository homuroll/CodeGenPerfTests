using System.IO;
using System.Linq;

using ProtoBuf;

namespace Serialization
{
    public class ProtoBufRunner<T> : IRunner
    {
        private readonly T[] objects;
        private byte[][] datas;
        private static readonly MemoryStream stream = new MemoryStream(128 * 1024);

        public ProtoBufRunner(T[] objects)
        {
            this.objects = objects;
            datas = objects.Select(obj =>
                {
                    stream.Position = 0;
                    stream.SetLength(0);
                    Serializer.Serialize<T>(stream, obj);
                    return stream.ToArray();
                }).ToArray();
        }

        public int Serialize()
        {
            int res = 0;
            foreach (var obj in objects)
            {
                stream.Position = 0;
                stream.SetLength(0);
                Serializer.Serialize(stream, obj);
                res += (int)stream.Position;
            }
            return res;
        }

        public object Deserialize()
        {
            T res = default(T);
            foreach (var data in datas)
                res = Serializer.Deserialize<T>(new MemoryStream(data));
            return res;
        }
    }
}