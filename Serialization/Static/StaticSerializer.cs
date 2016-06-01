using System;
using System.Collections.Generic;

namespace Serialization.Static
{
    public class StaticSerializer : ISerializer
    {
        public unsafe StaticSerializer()
        {
            writersAndSizeCounters.Add(typeof(Room),
                                       new Tuple<Delegate, Delegate>((SizeCounterDelegate<Room>)RoomWriter.GetSize,
                                                                     (WriterDelegate<Room>)RoomWriter.Write));
            writersAndSizeCounters.Add(typeof(Flat),
                                       new Tuple<Delegate, Delegate>((SizeCounterDelegate<Flat>)FlatWriter.GetSize,
                                                                     (WriterDelegate<Flat>)FlatWriter.Write));
            readers.Add(typeof(Room), (ReaderDelegate<Room>)RoomReader.Read);
            readers.Add(typeof(Flat), (ReaderDelegate<Flat>)FlatReader.Read);
        }

        public unsafe byte[] Serialize<T>(T obj)
        {
            var tuple = writersAndSizeCounters[typeof(T)];
            var result = new byte[((SizeCounterDelegate<T>)tuple.Item1)(obj)];
            fixed(byte* b = &result[0])
            {
                int index = 0;
                ((WriterDelegate<T>)tuple.Item2)(obj, b, ref index);
            }
            return result;
        }

        public unsafe T Deserialize<T>(byte[] data)
        {
            var reader = (ReaderDelegate<T>)readers[typeof(T)];

            fixed(byte* b = &data[0])
            {
                int index = 0;
                return reader(b, ref index);
            }
        }

        private readonly Dictionary<Type, Tuple<Delegate, Delegate>> writersAndSizeCounters = new Dictionary<Type, Tuple<Delegate, Delegate>>();
        private readonly Dictionary<Type, Delegate> readers = new Dictionary<Type, Delegate>();
    }

    public delegate int SizeCounterDelegate<T>(T obj);

    public unsafe delegate void WriterDelegate<T>(T obj, byte* data, ref int index);

    public unsafe delegate T ReaderDelegate<T>(byte* data, ref int index);
}