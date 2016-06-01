using System.Linq;

namespace Serialization
{
    public class SerializerRunner
    {
        public SerializerRunner(Flat[] flats, ISerializer serializer)
        {
            this.flats = flats;
            this.serializer = serializer;
            serializedFlats = flats.Select(flat => serializer.Serialize<Flat>(flat)).ToArray();
        }

        public int Serialize()
        {
            int res = 0;
            foreach(var flat in flats)
            {
                var bytes = serializer.Serialize(flat);
                res += bytes.Length;
            }
            return res;
        }

        public object Deserialize()
        {
            Flat res = null;
            foreach(var bytes in serializedFlats)
                res = serializer.Deserialize<Flat>(bytes);
            return res;
        }

        private readonly Flat[] flats;
        private readonly ISerializer serializer;
        private readonly byte[][] serializedFlats;
    }
}