using System;
using System.Linq;
using System.Reflection;

namespace Serialization.Dynamic
{
    public class ClassReader : IReader
    {
        private readonly Type type;
        private FieldInfo[] fields;
        private IReader[] fieldReaders;

        public ClassReader(Type type)
        {
            this.type = type;
            fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            fieldReaders = fields.Select(field => ReaderCollection.GetReader(field.FieldType)).ToArray();
        }

        public unsafe object Read(byte* data, ref int index)
        {
            if(data[index++] == 0)
                return null;
            var res = Activator.CreateInstance(type);
            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(res, fieldReaders[i].Read(data, ref index));
            return res;
        }
    }
}