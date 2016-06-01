namespace Serialization.Static
{
    public static unsafe class RoomWriter
    {
        public static int GetSize(Room room)
        {
            if(room == null) return 1;
            return 1 + 4 * 3; // 3 ints
        }

        public static void Write(Room room, byte* data, ref int index)
        {
            data[index++] = (byte)(room != null ? 1 : 0);
            if(room == null) return;
            *(int*)(data + index) = room.NumberOfWindows;
            index += 4;
            *(int*)(data + index) = room.NumberOfDoors;
            index += 4;
            *(int*)(data + index) = room.Area;
            index += 4;
        }
    }
}