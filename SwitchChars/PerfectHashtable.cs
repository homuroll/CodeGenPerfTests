using System;
using System.Collections.Generic;

namespace SwitchChars
{
    public class PerfectHashtable<T>
    {
        public PerfectHashtable(char[] keys, T[] values)
        {
            int n = CalcSize(keys);
            this.keys = new char[n];
            this.values = new T[n];
            for(int i = 0; i < keys.Length; ++i)
            {
                var key = keys[i];
                var idx = key % n;
                if(idx < 0) idx += n;
                this.keys[idx] = key;
                this.values[idx] = values[i];
            }
        }

        public bool TryGetValue(char key, out T value)
        {
            var idx = key % keys.Length;
            if(keys[idx] == 0 || keys[idx] != key)
            {
                value = default(T);
                return false;
            }
            value = values[idx];
            return true;
        }

        private static int CalcSize(char[] values)
        {
            var hashSet = new HashSet<int>();
            for(var n = Math.Max(values.Length, 1);; ++n)
            {
                hashSet.Clear();
                bool ok = true;
                foreach(var x in values)
                {
                    var item = x % n;
                    if(item < 0) item += n;
                    if(hashSet.Contains(item))
                    {
                        ok = false;
                        break;
                    }
                    hashSet.Add(item);
                }
                if(ok) return n;
            }
        }

        private readonly char[] keys;
        private readonly T[] values;
    }
}