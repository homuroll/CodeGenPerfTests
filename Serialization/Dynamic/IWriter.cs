namespace Serialization.Dynamic
{
    public unsafe interface IWriter
    {
        int GetSize(object obj);
        void Write(object obj, byte* data, ref int index);
    }
}