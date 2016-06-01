using System;
using System.Collections.Generic;

namespace Serialization.Dynamic
{
    public static class WriterCollection
    {
        public static IWriter GetWriter(Type type)
        {
            IWriter writer;
            if(!cache.TryGetValue(type, out writer))
                cache.Add(type, writer = GetWriterInternal(type));
            return writer;
        }

        private static IWriter GetWriterInternal(Type type)
        {
            if (type == typeof(int))
                return new Int32Writer();
            return new ClassWriter(type);
        }

        // Thread unsafe for simplicity
        private static readonly Dictionary<Type, IWriter> cache = new Dictionary<Type, IWriter>();
    }
}