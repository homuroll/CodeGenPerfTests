namespace Serialization.Dynamic
{
    public class Int32Reader: IReader
    {
        public unsafe object Read(byte* data, ref int index)
        {
            var res = *(int*)(data + index);
            index += 4;
            return res;
        }
    }
}