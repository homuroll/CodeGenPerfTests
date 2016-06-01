using System;
using System.Collections.Generic;

namespace SwitchStrings
{
    public class RoslynHashEvaluator
    {
        public RoslynHashEvaluator(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public unsafe int ComputeStringHash(string s)
        {
            if(s == null) return 0;
            int res = a;
            int b = this.b;
            fixed(char* c = s)
            {
                var len = s.Length;
                char* z = c;
                // From String.cs:
                // This depends on the fact that the String objects are
                // always zero terminated and that the terminating zero is not included 
                // in the length. For odd string sizes, the last compare will include 
                // the zero terminator.
                while(len > 0)
                {
                    res = (res ^ *(int*)z) * b;
                    z += 2;
                    len -= 2;
                }
            }
            return res;
        }

        public static RoslynHashEvaluator Create(string[] keys)
        {
            var random = new Random();
            while(true)
            {
                int a = random.Next();
                int b = random.Next();
                var result = new RoslynHashEvaluator(a, b);
                var hashSet = new HashSet<int>();
                bool ok = true;
                foreach(var key in keys)
                {
                    var hashCode = result.ComputeStringHash(key);
                    if(hashSet.Contains(hashCode))
                    {
                        ok = false;
                        break;
                    }
                    hashSet.Add(hashCode);
                }
                if(ok) return result;
            }
        }

        private readonly int a;
        private readonly int b;
    }
}