using System;
using System.Reflection.Emit;

using GrEmit;

namespace SwitchChars
{
    public static class UnrolledBinarySearchBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(char[] keys, T[] values)
        {
            // Assuming keys are sorted
            var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(bool),
                                           new[] {typeof(T[]), typeof(char), typeof(T).MakeByRefType()}, typeof(string), true);
            //var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("zzz"), AssemblyBuilderAccess.RunAndSave);
            //var module = assembly.DefineDynamicModule("qxx", "zzz.dll");
            //var typeBuilder = module.DefineType("Zzz", TypeAttributes.Class | TypeAttributes.Public);
            //var method = typeBuilder.DefineMethod("do_switch", MethodAttributes.Public | MethodAttributes.Static,
            //                                      typeof(bool), new[] {typeof(T[]), typeof(char), typeof(T).MakeByRefType()});
            int n = keys.Length;
            using(var il = new GroboIL(method))
            {
                var retFalseLabel = il.DefineLabel("retFalse");
                var context = new EmittingContext
                    {
                        Il = il,
                        Keys = keys,
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
            //typeBuilder.CreateType();
            //assembly.Save("zzz.dll");
            //return null;
            return (TryGetValueDelegate<T>)method.CreateDelegate(typeof(TryGetValueDelegate<T>), values);
        }

        private static void DoBinarySearch<T>(EmittingContext context, int l, int r)
        {
            var il = context.Il;
            if(l > r)
            {
                il.Br(context.RetFalseLabel);
                return;
            }
            if(r - l + 1 <= 3)
            {
                // just bunch of ifs
                for(; l <= r; l++)
                {
                    il.Ldarg(1); // stack: [key]
                    il.Ldc_I4(context.Keys[l]); // stack: [key, keys[l]]
                    var nextLabel = il.DefineLabel("next");
                    il.Bne_Un(nextLabel); // if(key != keys[l]) goto next; stack: []
                    il.Ldarg(2); // stack: [ref value]
                    il.Ldarg(0); // stack: [ref value, values]
                    il.Ldc_I4(l); // stack: [ref value, values, l]
                    il.Ldelem(typeof(T)); // stack: [ref value, values[l]]
                    il.Stind(typeof(T)); // value = values[l]; stack: []
                    il.Ldc_I4(1); // stack: [true]
                    il.Ret();
                    il.MarkLabel(nextLabel);
                }
                il.Br(context.RetFalseLabel);
            }
            else
            {
                int m = (l + r) / 2;

                il.Ldarg(1); // stack: [key]
                il.Ldc_I4(context.Keys[m]); // stack: [key, keys[m]]
                var nextLabel = il.DefineLabel("next");
                il.Bne_Un(nextLabel); // if(key != keys[m]) goto next; stack: []
                il.Ldarg(2); // stack: [ref value]
                il.Ldarg(0); // stack: [ref value, values]
                il.Ldc_I4(m); // stack: [ref value, values, m]
                il.Ldelem(typeof(T)); // stack: [ref value, values[m]]
                il.Stind(typeof(T)); // value = values[m]; stack: []
                il.Ldc_I4(1); // stack: [true]
                il.Ret();

                il.MarkLabel(nextLabel);
                il.Ldarg(1); // stack: [key]
                il.Ldc_I4(context.Keys[m]); // stack: [key, keys[m]]
                var goLeftLabel = il.DefineLabel("goLeft");
                il.Blt(goLeftLabel, false); // if(key < keys[m]]) goto goLeft; stack: []
                DoBinarySearch<T>(context, m + 1, r);

                il.MarkLabel(goLeftLabel);
                DoBinarySearch<T>(context, l, m - 1);
            }
        }

        private class EmittingContext
        {
            public GroboIL Il { get; set; }
            public char[] Keys { get; set; }
            public GroboIL.Label RetFalseLabel { get; set; }
        }
    }
}