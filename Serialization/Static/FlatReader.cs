namespace Serialization.Static
{
    public static unsafe class FlatReader
    {
        public static Flat Read(byte* data, ref int index)
        {
            if (data[index++] == 0)
                return null;
            var flat = new Flat();
            flat.Number = *(int*)(data + index);
            index += 4;
            flat.Kitchen = RoomReader.Read(data, ref index);
            flat.Room = RoomReader.Read(data, ref index);
            return flat;
        }
    }
}