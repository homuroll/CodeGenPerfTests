namespace Serialization.Static
{
    public static unsafe class RoomReader
    {
        public static Room Read(byte* data, ref int index)
        {
            if(data[index++] == 0)
                return null;
            var room = new Room();
            room.NumberOfWindows = *(int*)(data + index);
            index += 4;
            room.NumberOfDoors = *(int*)(data + index);
            index += 4;
            room.Area = *(int*)(data + index);
            index += 4;
            return room;
        }
    }
}