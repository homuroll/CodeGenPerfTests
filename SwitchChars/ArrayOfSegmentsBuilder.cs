using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using GrEmit;
using GrEmit.Utils;

namespace SwitchChars
{
    public static class ArrayOfSegmentsBuilder
    {
        public static TryGetValueDelegate<T> Build<T>(char[] keys, T[] values, int numberOfSegments, int numberOfKeysPerSegment)
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(bool),
                                           new[] {typeof(Closure<T>), typeof(char), typeof(T).MakeByRefType()}, typeof(string), true);
            var indices = new List<int>();
            using(var il = new GroboIL(method))
            {
                var idx = il.DeclareLocal(typeof(int), "idx");
                var retFalseLabel = il.DefineLabel("retFalse");
                for(int i = 0; i < numberOfSegments; ++i)
                {
                    var firstKeyInSegment = keys[i * numberOfKeysPerSegment];
                    var lastKeyInSegment = keys[numberOfKeysPerSegment - 1 + i * numberOfKeysPerSegment];
                    il.Ldarg(1); // stack: [key]
                    il.Ldc_I4(firstKeyInSegment); // stack: [key, firstKey]
                    var nextSegmentLabel = il.DefineLabel("nextSegment");
                    il.Blt(nextSegmentLabel, false); // if(key < firstKey) goto nextSegment; stack: []
                    il.Ldarg(1); // stack: [key]
                    il.Ldc_I4(lastKeyInSegment); // stack: [key, lastKey]
                    il.Bgt(nextSegmentLabel, false); // if(key > lastKey) goto nextSegment; stack: []
                    il.Ldarg(0); // stack: [closure]
                    il.Ldfld(HackHelpers.GetField<Closure<T>>(x => x.indices)); // stack: [closure.indices]
                    il.Ldarg(1); // stack: [closure.indices, key]
                    il.Ldc_I4(firstKeyInSegment - indices.Count); // stack: [closure.indices, key, diff]
                    il.Sub(); // stack: [closure.indices, key - diff]
                    il.Ldelem(typeof(int)); // stack: [closure.indices[key - diff]]
                    il.Dup();
                    il.Stloc(idx); // idx = closure.indices[key - diff]; stack: [idx]
                    il.Ldc_I4(0); // stack: [idx, 0]
                    il.Blt(retFalseLabel, false); // if(idx < 0) goto retFalse; stack: []
                    il.Ldarg(2); // stack: [ref value]
                    il.Ldarg(0); // stack: [ref value, closure]
                    il.Ldfld(HackHelpers.GetField<Closure<T>>(x => x.values)); // stack: [ref value, closure.values]
                    il.Ldloc(idx); // stack: [ref value, closure.values, idx]
                    il.Ldelem(typeof(T)); // stack: [ref value, closure.values[idx]]
                    il.Stind(typeof(T)); // value = closure.values[idx]; stack: []
                    il.Ldc_I4(1); // stack: [true]
                    il.Ret();
                    il.MarkLabel(nextSegmentLabel);
                    var segmentLength = lastKeyInSegment - firstKeyInSegment + 1;
                    int start = indices.Count;
                    for(int j = 0; j < segmentLength; ++j)
                        indices.Add(-1);
                    for(int j = 0; j < numberOfKeysPerSegment; ++j)
                        indices[start + keys[i * numberOfKeysPerSegment + j] - firstKeyInSegment] = i * numberOfKeysPerSegment + j;
                }
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
                    indices = indices.ToArray(),
                    values = values
                };
            return (TryGetValueDelegate<T>)method.CreateDelegate(typeof(TryGetValueDelegate<T>), closure);
        }

        private class Closure<T>
        {
            public int[] indices;
            public T[] values;
        }
    }
}