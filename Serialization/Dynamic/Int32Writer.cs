namespace Serialization.Dynamic
{
    public class Int32Writer : IWriter
    {
        public int GetSize(object obj)
        {
            return 4;
        }

        public unsafe void Write(object obj, byte* data, ref int index)
        {
            *(int*)(data + index) = (int)obj;
            index += 4;
        }
    }
}