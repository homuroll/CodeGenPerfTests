using System;
using System.Linq;
using System.Reflection;

namespace Serialization.Dynamic
{
    public class ClassWriter : IWriter
    {
        private FieldInfo[] fields;
        private IWriter[] fieldWriters;

        public ClassWriter(Type type)
        {
            fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            fieldWriters = fields.Select(field => WriterCollection.GetWriter(field.FieldType)).ToArray();
        }

        public unsafe void Write(object obj, byte* data, ref int index)
        {
            data[index++] = (byte)(obj != null ? 1 : 0);
            if(obj != null)
            {
                for (int i = 0; i < fields.Length; i++)
                    fieldWriters[i].Write(fields[i].GetValue(obj), data, ref index);
            }
        }

        public int GetSize(object obj)
        {
            int res = 1;
            if(obj != null)
            {
                for(int i = 0; i < fields.Length; i++)
                    res += fieldWriters[i].GetSize(fields[i].GetValue(obj));
            }
            return res;
        }
    }
}