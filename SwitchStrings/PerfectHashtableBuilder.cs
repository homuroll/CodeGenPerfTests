using System;
using System.Collections.Generic;

namespace SwitchStrings
{
    public static class PerfectHashtableBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(string[] keys, T[] values, Func<string, int> hashEvaluator)
        {
            return new PerfectHashtable<T>(keys, values, hashEvaluator).TryGetValue;
        }

        private class PerfectHashtable<T>
        {
            public PerfectHashtable(string[] keys, T[] values, Func<string, int> hashEvaluator)
            {
                this.hashEvaluator = hashEvaluator;
                int n = CalcSize(keys);
                this.keys = new string[n];
                this.values = new T[n];
                for(int i = 0; i < keys.Length; ++i)
                {
                    var key = keys[i];
                    var idx = hashEvaluator(key) % n;
                    if(idx < 0) idx += n;
                    this.keys[idx] = key;
                    this.values[idx] = values[i];
                }
            }

            public bool TryGetValue(string key, out T value)
            {
                var idx = hashEvaluator(key) % keys.Length;
                if(idx < 0) idx += keys.Length;
                if(keys[idx] == null || keys[idx] != key)
                {
                    value = default(T);
                    return false;
                }
                value = values[idx];
                return true;
            }

            private int CalcSize(string[] values)
            {
                var hashSet = new HashSet<int>();
                for(var n = Math.Max(values.Length, 1);; ++n)
                {
                    hashSet.Clear();
                    bool ok = true;
                    foreach(var x in values)
                    {
                        var item = hashEvaluator(x) % n;
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

            private readonly Func<string, int> hashEvaluator;

            private readonly string[] keys;
            private readonly T[] values;
        }
    }
}