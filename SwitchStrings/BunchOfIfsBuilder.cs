using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using GrEmit;

namespace SwitchStrings
{
    public static class BunchOfIfsBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(string[] keys, T[] values)
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(bool),
                                           new[] {typeof(T[]), typeof(string), typeof(T).MakeByRefType()}, typeof(string), true);
            using(var il = new GroboIL(method))
            {
                for(int i = 0; i < keys.Length; ++i)
                {
                    il.Ldarg(1); // stack: [key]
                    il.Ldstr(keys[i]); // stack: [key, keys[i]]
                    var nextKeyLabel = il.DefineLabel("nextKey");
                    il.Call(stringEqualityOperator); // stack: [key == keys[i]]
                    il.Brfalse(nextKeyLabel); // if(key != keys[i]) goto nextKey; stack: []
                    il.Ldarg(2); // stack: [ref value]
                    il.Ldarg(0); // stack: [ref value, values]
                    il.Ldc_I4(i); // stack: [ref value, values, i]
                    il.Ldelem(typeof(T)); // stack: [ref value, values[i]]
                    il.Stind(typeof(T)); // value = values[i]; stack: []
                    il.Ldc_I4(1); // stack: [true]
                    il.Ret();
                    il.MarkLabel(nextKeyLabel);
                }
                il.Ldarg(2); // stack: [ref value]
                if(typeof(T).IsValueType)
                    il.Initobj(typeof(T)); // value = default(T); stack: []
                else
                {
                    il.Ldnull(); // stack: [ref value, null]
                    il.Stind(typeof(T)); // value = null; stack: []
                }
                il.Ldc_I4(0); // stack: [false]
                il.Ret();
            }
            return (TryGetValueDelegate<T>)method.CreateDelegate(typeof(TryGetValueDelegate<T>), values);
        }

        private static readonly MethodInfo stringEqualityOperator = ((BinaryExpression)((Expression<Func<string, bool>>)(s => s == "zzz")).Body).Method;
    }
}