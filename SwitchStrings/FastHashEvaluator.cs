using System;
using System.Collections.Generic;

namespace SwitchStrings
{
    public static class FastHashEvaluator
    {
        public static Func<string, int> Create(string[] keys)
        {
            var fastHashEvaluator1 = FastHashEvaluator1.Create(keys);
            if(fastHashEvaluator1 != null)
                return fastHashEvaluator1.ComputeStringHash;
            var fastHashEvaluator2 = FastHashEvaluator2.Create(keys);
            if(fastHashEvaluator2 != null)
                return fastHashEvaluator2.ComputeStringHash;
            var fastHashEvaluator3 = FastHashEvaluator3.Create(keys);
            if(fastHashEvaluator3 != null)
                return fastHashEvaluator3.ComputeStringHash;
            return RoslynHashEvaluator.Create(keys).ComputeStringHash;
        }

        private class FastHashEvaluator1
        {
            public FastHashEvaluator1(int k)
            {
                this.k = k;
            }

            public int ComputeStringHash(string s)
            {
                return s == null || k >= s.Length ? 0 : s[k];
            }

            public static FastHashEvaluator1 Create(string[] keys)
            {
                for(int k = 0; k < 10; ++k)
                {
                    bool ok = true;
                    for(int i = 0; i < keys.Length && ok; ++i)
                    {
                        var key = keys[i];
                        int x = k >= key.Length ? 0 : key[k];
                        for(int j = i + 1; j < keys.Length; ++j)
                        {
                            int y = k >= keys[j].Length ? 0 : keys[j][k];
                            if(x == y)
                            {
                                ok = false;
                                break;
                            }
                        }
                    }
                    if(ok)
                        return new FastHashEvaluator1(k);
                }
                return null;
            }

            private readonly int k;
        }

        private class FastHashEvaluator2
        {
            public FastHashEvaluator2(int k, int l)
            {
                this.k = k;
                this.l = l;
            }

            public int ComputeStringHash(string s)
            {
                if(s == null) return 0;
                return (k >= s.Length ? 0 : s[k]) | ((l >= s.Length ? 0 : s[l]) << 16);
            }

            public static FastHashEvaluator2 Create(string[] keys)
            {
                for(int k = 0; k < 10; ++k)
                {
                    for(int l = k + 1; l < 10; ++l)
                    {
                        bool ok = true;
                        for(int i = 0; i < keys.Length && ok; ++i)
                        {
                            var key = keys[i];
                            int x = (k >= key.Length ? 0 : key[k]) | ((l >= key.Length ? 0 : key[l]) << 16);
                            for(int j = i + 1; j < keys.Length; ++j)
                            {
                                int y = (k >= keys[j].Length ? 0 : keys[j][k]) | ((l >= keys[j].Length ? 0 : keys[j][l]) << 16);
                                if(x == y)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                        if(ok)
                            return new FastHashEvaluator2(k, l);
                    }
                }
                return null;
            }

            private readonly int k;
            private readonly int l;
        }

        private class FastHashEvaluator3
        {
            public FastHashEvaluator3(int k, int l, int m, int z)
            {
                this.k = k;
                this.l = l;
                this.m = m;
                this.z = z;
            }

            public int ComputeStringHash(string s)
            {
                if (s == null) return 0;
                return ((k >= s.Length ? 0 : s[k]) | ((l >= s.Length ? 0 : s[l]) << 16)) + (m >= s.Length ? 0 : s[m]) * z;
            }

            public static FastHashEvaluator3 Create(string[] keys)
            {
                for (int k = 0; k < 10; ++k)
                {
                    for (int l = k + 1; l < 10; ++l)
                    {
                        for(int m = l + 1; m < 10; ++m)
                        {
                            bool ok = true;
                            for(int i = 0; i < keys.Length && ok; ++i)
                            {
                                var key = keys[i];
                                int x1 = (k >= key.Length ? 0 : key[k]) | ((l >= key.Length ? 0 : key[l]) << 16);
                                int x2 = m >= key.Length ? 0 : key[m];
                                for (int j = i + 1; j < keys.Length; ++j)
                                {
                                    int y1 = (k >= keys[j].Length ? 0 : keys[j][k]) | ((l >= keys[j].Length ? 0 : keys[j][l]) << 16);
                                    int y2 = m >= keys[j].Length ? 0 : keys[j][m];
                                    if (x1 == y1 && x2 == y2)
                                    {
                                        ok = false;
                                        break;
                                    }
                                }
                            }
                            if(ok)
                            {
                                var random = new Random();
                                while(true)
                                {
                                    int z = random.Next();
                                    var hashset = new HashSet<int>();
                                    ok = true;
                                    foreach(var key in keys)
                                    {
                                        var hashCode = ((k >= key.Length ? 0 : key[k]) | ((l >= key.Length ? 0 : key[l]) << 16))
                                            + (m >= key.Length ? 0 : key[m]) * z;
                                        if(hashset.Contains(hashCode))
                                        {
                                            ok = false;
                                            break;
                                        }
                                        hashset.Add(hashCode);
                                    }
                                    if(ok)
                                        return new FastHashEvaluator3(k, l, m, z);
                                }
                                
                            }
                        }
                    }
                }
                return null;
            }

            private readonly int k;
            private readonly int l;
            private readonly int m;
            private readonly int z;
        }

    }
}