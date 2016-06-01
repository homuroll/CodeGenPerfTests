namespace Serialization.Static
{
    public static unsafe class FlatWriter
    {
        public static int GetSize(Flat flat)
        {
            if(flat == null) return 1;
            return 1 + 4 + RoomWriter.GetSize(flat.Kitchen) + RoomWriter.GetSize(flat.Room);
        }

        public static void Write(Flat flat, byte* data, ref int index)
        {
            data[index++] = (byte)(flat != null ? 1 : 0);
            if (flat == null) return;
            *(int*)(data + index) = flat.Number;
            index += 4;
            RoomWriter.Write(flat.Kitchen, data, ref index);
            RoomWriter.Write(flat.Room, data, ref index);
        }
    }
}