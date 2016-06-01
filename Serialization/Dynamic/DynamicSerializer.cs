namespace Serialization.Dynamic
{
    public class DynamicSerializer : ISerializer
    {
        public unsafe T Deserialize<T>(byte[] data)
        {
            var reader = ReaderCollection.GetReader(typeof(T));
            fixed(byte* b = &data[0])
            {
                int index = 0;
                return (T)reader.Read(b, ref index);
            }
        }

        public unsafe byte[] Serialize<T>(T obj)
        {
            var writer = WriterCollection.GetWriter(typeof(T));
            var res = new byte[writer.GetSize(obj)];
            fixed(byte* b = &res[0])
            {
                int index = 0;
                writer.Write(obj, b, ref index);
            }
            return res;
        }
    }
}