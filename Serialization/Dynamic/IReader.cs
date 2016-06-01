namespace Serialization.Dynamic
{
    public unsafe interface IReader
    {
        object Read(byte* data, ref int index);
    }
}