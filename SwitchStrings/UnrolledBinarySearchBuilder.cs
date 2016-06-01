using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using GrEmit;
using GrEmit.Utils;

namespace SwitchStrings
{
    public static class UnrolledBinarySearchBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(string[] keys, T[] values, Func<string, int> hashCodeEvaluator)
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(bool),
                                           new[] {typeof(Closure<T>), typeof(string), typeof(T).MakeByRefType()}, typeof(string), true);
            var hashCodes = keys.Select(hashCodeEvaluator).ToArray();
            int n = keys.Length;
            var indexes = new int[n];
            for(int i = 0; i < n; ++i)
                indexes[i] = i;
            Array.Sort(indexes, (lhs, rhs) => hashCodes[lhs].CompareTo(hashCodes[rhs]));
            using(var il = new GroboIL(method))
            {
                var retFalseLabel = il.DefineLabel("retFalse");
                var hashCode = il.DeclareLocal(typeof(int), "hashCode");
                il.Ldarg(0); // stack: [closure]
                il.Ldfld(HackHelpers.GetField<Closure<T>>(x => x.hashCodeEvaluator)); // stack: [closure.hashCodeEvaluator]
                il.Ldarg(1); // stack: [closure.hashCodeEvaluator, key]
                il.Call(typeof(Func<string, int>).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public)); // stack: [closure.hashCodeEvaluator(key)]
                il.Stloc(hashCode); // hashCode = closure.hashCodeEvaluator(key); stack: []

                var context = new EmittingContext
                    {
                        Il = il,
                        Keys = keys,
                        HashCodes = hashCodes,
                        Indexes = indexes,
                        HashCode = hashCode,
                        RetFalseLabel = retFalseLabel
                    };
                DoBinarySearch<T>(context, 0, n - 1);
                il.MarkLabel(retFalseLabel);
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
            var closure = new Closure<T>
                {
                    hashCodeEvaluator = hashCodeEvaluator,
                    values = values
                };
            return (TryGetValueDelegate<T>)method.CreateDelegate(typeof(TryGetValueDelegate<T>), closure);
        }

        private static void DoBinarySearch<T>(EmittingContext context, int l, int r)
        {
            var il = context.Il;
            if(l > r)
            {
                il.Br(context.RetFalseLabel);
                return;
            }

            int m = (l + r) / 2;

            il.Ldloc(context.HashCode); // stack: [hashCode]
            il.Ldc_I4(context.HashCodes[context.Indexes[m]]); // stack: [hashCode, hashCodes[m]]
            var nextLabel = il.DefineLabel("next");
            il.Bne_Un(nextLabel); // if(hashCode != hashCodes[m]) goto next; stack: []
            il.Ldarg(1); // stack: [key]
            il.Ldstr(context.Keys[context.Indexes[m]]); // stack: [key, keys[m]]
            il.Call(stringEqualityOperator); // stack: [key == keys[m]]
            il.Brfalse(context.RetFalseLabel); // if(key != keys[m]) goto retFalse; stack: []
            il.Ldarg(2); // stack: [ref value]
            il.Ldarg(0); // stack: [ref value, closure]
            il.Ldfld(HackHelpers.GetField<Closure<T>>(x => x.values)); // stack: [ref value, closure.values]
            il.Ldc_I4(context.Indexes[m]); // stack: [ref value, closure.values, m]
            il.Ldelem(typeof(T)); // stack: [ref value, closure.values[m]]
            il.Stind(typeof(T)); // value = closure.values[m]; stack: []
            il.Ldc_I4(1); // stack: [true]
            il.Ret();

            il.MarkLabel(nextLabel);
            il.Ldloc(context.HashCode); // stack: [hashCode]
            il.Ldc_I4(context.HashCodes[context.Indexes[m]]); // stack: [hashCode, hashCodes[m]]
            var goLeftLabel = il.DefineLabel("goLeft");
            il.Blt(goLeftLabel, false); // if(hashCode < hashCodes[m]]) goto goLeft; stack: []
            DoBinarySearch<T>(context, m + 1, r);

            il.MarkLabel(goLeftLabel);
            DoBinarySearch<T>(context, l, m - 1);
        }

        private static readonly MethodInfo stringEqualityOperator = ((BinaryExpression)((Expression<Func<string, bool>>)(s => s == "zzz")).Body).Method;

        private class Closure<T>
        {
            public Func<string, int> hashCodeEvaluator;
            public T[] values;
        }

        private class EmittingContext
        {
            public GroboIL Il { get; set; }
            public string[] Keys { get; set; }
            public int[] HashCodes { get; set; }
            public int[] Indexes { get; set; }
            public GroboIL.Local HashCode { get; set; }
            public GroboIL.Label RetFalseLabel { get; set; }
        }
    }
}