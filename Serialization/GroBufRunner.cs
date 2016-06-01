using System.Linq;

using GroBuf;
using GroBuf.DataMembersExtracters;

namespace Serialization
{
    public interface IRunner
    {
        int Serialize();
        object Deserialize();
    }

    public class GroBufRunner<T> : IRunner
    {
        private readonly T[] objects;
        private Serializer serializer;
        private byte[][] datas;

        public GroBufRunner(T[] objects)
        {
            this.objects = objects;
            serializer = new Serializer(new PropertiesExtractor());
            datas = objects.Select(obj => serializer.Serialize<T>(obj)).ToArray();
            foreach(var data in datas)
                serializer.Deserialize<T>(data);
        }

        public int Serialize()
        {
            int res = 0;
            foreach(var obj in objects)
            {
                var data = serializer.Serialize(obj);
                res += data.Length;
            }
            return res;
        }

        public object Deserialize()
        {
            T res = default(T);
            for(int i = 0; i < datas.Length; i++)
            {
                res = serializer.Deserialize<T>(datas[i]);
            }
            return res;
        }
    }
}