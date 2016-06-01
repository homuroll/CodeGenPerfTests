using System;
using System.Collections.Generic;

namespace Serialization.Dynamic
{
    public static class ReaderCollection
    {
        public static IReader GetReader(Type type)
        {
            IReader reader;
            if (!cache.TryGetValue(type, out reader))
                cache.Add(type, reader = GetReaderInternal(type));
            return reader;
        }

        private static IReader GetReaderInternal(Type type)
        {
            if (type == typeof(int))
                return new Int32Reader();
            return new ClassReader(type);
        }

        // Thread unsafe for simplicity
        private static readonly Dictionary<Type, IReader> cache = new Dictionary<Type, IReader>();
    }
}